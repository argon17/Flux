namespace AstGenerator;

public static class AstGenerator
{
    public static void Main(string[] args)
    {
        DefineAst(args[0], "Expr",
        [
            "Binary: Expr left, Token @operator, Expr right",
            "Grouping: Expr expression",
            "Literal: Object value",
            "Unary: Token @operator, Expr right",
            "Variable: Token name",
        ]);
        
        DefineAst(args[0], "Stmt",
        [
            "fcExpression: Expr expression",
            "Print: Expr expression",
            "Var: Token name, Expr? initializer",
        ], false);
    }

    private static void DefineAst(string outDir, string baseName, List<string> types, bool returnsValue = true)
    {
        string path = Path.Combine(outDir, $"{baseName}.g.cs");
        using StreamWriter writer = new StreamWriter(path);

        writer.WriteLine("namespace FluxInterpreter;");
        writer.WriteLine();
        writer.WriteLine($"public abstract class {baseName}");
        writer.WriteLine("{");

        DefineVisitor(writer, baseName, types, returnsValue);

        foreach (string type in types)
        {
            string className = type.Split(":")[0].Trim();
            string fields = type.Split(":")[1].Trim();
            DefineType(writer, baseName, className, fields, returnsValue);
        }

        writer.WriteLine();
        if (returnsValue)
        {
            writer.WriteLine("    public abstract T Accept<T>(IVisitor<T> visitor);");
        }
        else
        {
            writer.WriteLine("    public abstract void Accept(IVisitor visitor);");
        }
        writer.WriteLine("}");
    }

    private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types, bool returnsValue)
    {
        if (returnsValue)
        {
            writer.WriteLine("    public interface IVisitor<T>");
            writer.WriteLine("    {");
            foreach (string type in types)
            {
                string typeName = type.Split(":")[0].Trim();
                writer.WriteLine($"        T Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }
            writer.WriteLine("    }");
        }
        else
        {
            writer.WriteLine("    public interface IVisitor");
            writer.WriteLine("    {");
            foreach (string type in types)
            {
                string typeName = type.Split(":")[0].Trim();
                writer.WriteLine($"        void Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }
            writer.WriteLine("    }");
        }
        writer.WriteLine();
    }

    private static void DefineType(StreamWriter writer, string baseName, string className, string fields, bool returnsValue)
    {
        writer.WriteLine($"    public class {className}({fields}) : {baseName}");
        writer.WriteLine("    {");

        // Write properties with initialization
        string[] fieldPairs = fields.Split(", ");
        foreach (string field in fieldPairs)
        {
            string[] parts = field.Trim().Split(' ');
            if (parts.Length >= 2)
            {
                string fieldType = parts[0];
                string fieldName = parts[1];
                
                // Convert parameter name to property name (capitalize first letter)
                string propertyName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);
                if (fieldName.StartsWith("@"))
                {
                    propertyName = char.ToUpper(fieldName[1]) + fieldName.Substring(2);
                }
                
                writer.WriteLine($"        public {fieldType} {propertyName} {{ get; }} = {fieldName};");
            }
        }

        writer.WriteLine();
        
        // Write Accept method implementation
        if (returnsValue)
        {
            writer.WriteLine($"        public override T Accept<T>(IVisitor<T> visitor)");
            writer.WriteLine("        {");
            writer.WriteLine($"            return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("        }");
        }
        else
        {
            writer.WriteLine($"        public override void Accept(IVisitor visitor)");
            writer.WriteLine("        {");
            writer.WriteLine($"            visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("        }");
        }

        writer.WriteLine("    }");
        writer.WriteLine();
    }
}