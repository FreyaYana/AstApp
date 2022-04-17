namespace ConsoleAppAst.Models
{
    public class LexemeBuffer {
        
        private int _pos;
        private readonly Lexeme[] _lexemes;

        public LexemeBuffer(Lexeme[] lexemes) {
            this._lexemes = lexemes;
        }

        public Lexeme Up() {
            return _lexemes[_pos++];
        }

        public void Down() {
            _pos--;
        }
        
        public int GetPosition() {
            return _pos;
        }
    }
}