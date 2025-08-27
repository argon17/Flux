using System.Globalization;

namespace FluxInterpreter;

public class Interpreter : Expr.IVisitor<object?>, Stmt.IVisitor
{
    private readonly Environment _environment = new();
    public void Interpret(List<Stmt> statements)
    {
        try
        {
            foreach (Stmt statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeError error)
        {
            Flux.RuntimeError(error);
        }
    }

    public void Interpret(Expr? expression)
    {
        try
        {
            object? value = Evaluate(expression);
            Console.WriteLine(Stringify(value));
        }
        catch (RuntimeError error)
        {
            Flux.RuntimeError(error);
        }
    }

    private void Execute(Stmt stmt)
    {
        stmt.Accept(this);
    }

    public void VisitfcExpressionStmt(Stmt.fcExpression stmt)
    {
        Evaluate(stmt.Expression);
    }

    public void VisitPrintStmt(Stmt.Print stmt)
    {
        object? value = Evaluate(stmt.Expression);
        Console.WriteLine(Stringify(value));
    }

    public void VisitVarStmt(Stmt.Var stmt)
    {
        object? value = null;
        if (stmt.Initializer != null)
        {
            value = Evaluate(stmt.Initializer);
        }

        _environment.Define(stmt.Name, value);
    }
    public object VisitVariableExpr(Expr.Variable expr)
    {
        return _environment.Get(expr.Name);
    }

    private string Stringify(object? value)
    {
        if (value == null) return "nil";
        if (value is double d)
        {
            string text = d.ToString(CultureInfo.InvariantCulture);
            if (text.EndsWith(".0"))
            {
                text = text.Substring(0, text.Length - 2);
            }
            return text;
        }
        return value.ToString() ?? "";
    }

    public object? VisitBinaryExpr(Expr.Binary expr)
    {
        object? left = Evaluate(expr.Left);
        object? right = Evaluate(expr.Right);
        if(left is null || right is null)
        {
            throw new RuntimeError(expr.Operator, "Operands must be non-null.");
        }

        switch (expr.Operator.Type)
        {
            case TokenType.Minus:
                CheckNumberOperands(expr.Operator, left, right);
                return (double)left - (double)right;
            case TokenType.Plus:
                if (left is double l && right is double r)
                {
                    return l + r;
                }

                if (left is string ls && right is string rs)
                {
                    return ls + rs;
                }

                throw new Exception("Operands must be two numbers or two strings.");
            case TokenType.Slash:
                CheckNumberOperands(expr.Operator, left, right);
                return (double)left / (double)right;
            case TokenType.Star:
                CheckNumberOperands(expr.Operator, left, right);
                return (double)left * (double)right;
            case TokenType.Greater:
                CheckNumberOperands(expr.Operator, left, right);
                return (double)left > (double)right;
            case TokenType.GreaterEqual:
                CheckNumberOperands(expr.Operator, left, right);
                return (double)left >= (double)right;
            case TokenType.Less:
                CheckNumberOperands(expr.Operator, left, right);
                return (double)left < (double)right;
            case TokenType.LessEqual:
                CheckNumberOperands(expr.Operator, left, right);
                return (double)left <= (double)right;
            case TokenType.EqualEqual:
                return IsEqual(left, right);
            case TokenType.NegEqual:
                return !IsEqual(left, right);
        }

        return null;
    }

    private bool IsEqual(object? left, object? right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;
        return left.Equals(right);
    }

    public object? VisitGroupingExpr(Expr.Grouping expr)
    {
        return Evaluate(expr.Expression);
    }

    private object? Evaluate(Expr expression)
    {
        return expression.Accept(this);
    }

    public object VisitLiteralExpr(Expr.Literal expr)
    {
        return expr.Value;
    }

    public object? VisitUnaryExpr(Expr.Unary expr)
    {
        object? right = Evaluate(expr.Right);
        switch (expr.Operator.Type)
        {
            case TokenType.Minus:
                CheckNumberOperand(expr.Operator, right);
                return -(double)right!;
            case TokenType.Neg:
                return !IsTruthy(right);
        }

        return null;
    }


    private void CheckNumberOperand(Token exprOperator, object? right)
    {
        if (right is double) return;
        throw new RuntimeError(exprOperator, "Operand must be a number.");
    }
    
    private void CheckNumberOperands(Token exprOperator, object? left, object? right)
    {
        if (left is double && right is double) return;
        throw new RuntimeError(exprOperator, "Operands must be numbers.");
    }

    private bool IsTruthy(object? @object)
    {
        if (@object == null) return false;
        if (@object is bool b) return b;
        return true;
    }
    
}