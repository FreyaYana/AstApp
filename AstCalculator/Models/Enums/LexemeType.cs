namespace ConsoleAppAst.Models.Enums
{
    /// <summary>
    /// Тип лексемы
    /// </summary>
    public enum LexemeType
    {
        /// <summary>
        /// Инициализирующий
        /// </summary>
        Unknown,
        
        /// <summary>
        /// ( Левая скобка
        /// </summary>
        LBracket, 
        
        /// <summary>
        /// ) Правая скобка
        /// </summary>
        RBracket,
        
        /// <summary>
        /// + Плюс
        /// </summary>
        Plus, 
        
        /// <summary>
        /// - Минус
        /// </summary>
        Minus,
        
        /// <summary>
        /// * Умножение
        /// </summary>
        Mul,
        
        /// <summary>
        /// / Деление
        /// </summary>
        Div,
        
        /// <summary>
        /// Операнд
        /// </summary>
        Number,
        
        /// <summary>
        /// Переменная
        /// </summary>
        Var,
        
        /// <summary>
        /// Конец строки
        /// </summary>
        Eof
    }
}