#nullable enable
namespace FluxInterpreter;

public abstract class Stmt
{
    public interface IVisitor
    {
        void VisitfcExpressionStmt(fcExpression stmt);
        void VisitPrintStmt(Print stmt);
        void VisitVarStmt(Var stmt);
    }

    public class fcExpression(Expr expression) : Stmt
    {
        public Expr Expression { get; } = expression;

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitfcExpressionStmt(this);
        }
    }

    public class Print(Expr expression) : Stmt
    {
        public Expr Expression { get; } = expression;

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitPrintStmt(this);
        }
    }

    public class Var(Token name, Expr? initializer) : Stmt
    {
        public Token Name { get; } = name;
        public Expr? Initializer { get; } = initializer;

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitVarStmt(this);
        }
    }


    public abstract void Accept(IVisitor visitor);
}
