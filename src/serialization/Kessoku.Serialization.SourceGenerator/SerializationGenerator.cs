using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Threading;

namespace Kessoku.Serialization.SourceGenerator
{
    public class SerilizationGenerator : IIncrementalGenerator
    {
        private const string SerilizationInterface = "Kessoku.Serialization.Types.ISerialization";
        private const string IgnoreMemberAttribute = "Kessoku.Serialization.Types.IgnoreMemberAttribute";
        private bool SerializationTargetPredicate(SyntaxNode node, CancellationToken cancellationToken)
        {
            bool isClass = node is ClassDeclarationSyntax;
            bool isStruct = node is StructDeclarationSyntax;

            bool isDerived = false;
            if(node is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                isDerived = typeDeclarationSyntax.BaseList != null;
            }

            return (isClass || isStruct) && isDerived;
        }
        /// <summary>
        /// Method for check, is INamedTypedSymbol contains ISerializable interface.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private bool IsSerializable(INamedTypeSymbol symbol)
        {
            return symbol.Interfaces.Any(a => a.ToDisplayString() == SerilizationInterface);
        }
        private Target SerializationTargetTransform(GeneratorSyntaxContext generatorSyntaxContext, CancellationToken token)
        {
            var typeSymbol = generatorSyntaxContext
                .SemanticModel.GetDeclaredSymbol(generatorSyntaxContext.Node) as INamedTypeSymbol;

            bool isSerializable = IsSerializable(typeSymbol);

            if (!isSerializable) {
                return null;
            }

            Target t = new Target
            {
                Namespace = typeSymbol.ContainingNamespace.ToDisplayString(),
                MemberName = typeSymbol.Name,
                //TODO: check support for records, enums and so on
                Type = typeSymbol.IsReferenceType ? "class" : "struct"
            };


            var symbolMembers = typeSymbol
                .GetMembers()
                .Where(a => !a.IsStatic)
                .Where(b => b.Kind == SymbolKind.Property || b.Kind == SymbolKind.Field)
                .Where(c=> !c.GetAttributes().Any(
                    d=>d.AttributeClass.ToDisplayString() == IgnoreMemberAttribute
                ))
                .OrderBy(e=>e.Name);

            var members =
                symbolMembers
                .Select(a=>
                    {
                        var m = new Member();
                        m.Name = a.Name;
                        string type;
                        if(a is IFieldSymbol ifs)
                        {
                            type = ifs.Type.ToDisplayString();
                        }
                        else if(a is IPropertySymbol ips)
                        {
                            type = ips.Type.ToDisplayString();
                        }
                        else
                        {
                            return null;
                        }
                        m.Type = type;
                        return m;
                    }
                )
                .Where(b=>b is not null);

            t.Members.AddRange(members);

            return t;
        }
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var serializationTarget = context.SyntaxProvider
                .CreateSyntaxProvider(
                    SerializationTargetPredicate,
                    SerializationTargetTransform
                );
        }
    }
}
