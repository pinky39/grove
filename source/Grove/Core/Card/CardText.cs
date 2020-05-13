namespace Grove
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Windows.Media;
  using Media;

  public class CardText : IEnumerable<Token>
  {
    private static readonly Regex Brackets = new Regex("{(.*)}", RegexOptions.Compiled);
    private static readonly string[] ManaSymbols = new[] { 
      "w", "u", "b", "r", "g", "wp", "up", "bp", "rp", "gp", "t", "x",
      "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11",
      "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8", "-9", "-10",
      "+1", "+2", "+3", "+4", "+5", "+6", "+7", "+8", "+9", "+10",
    };
    
    private static readonly List<Func<string, Token>> TokenFactoy = new List<Func<string, Token>>
      {
        token => token == "EOL" ? new EolToken() : null,
        token =>
          {
            token = token.ToLowerInvariant();

            if (ManaSymbols.Contains(token))
            {
              return new ManaSymbolToken(token);
            }
            return null;
          },

          token =>
          {
              token = token.ToLowerInvariant();

              if(token == "i")
                  return new ReminderTextOpenToken();

              if(token == "/i")
                  return new ReminderTextCloseToken();

              return null;
          },
      };

    private static readonly Regex Tokenizer = new Regex("( )|({[^}]*})", RegexOptions.Compiled);
    private readonly string _str;

    public CardText(string text)
    {
      if (String.IsNullOrEmpty(text))
      {
        Tokens = new List<Token> {};
        _str = String.Empty;
        return;
      }

      text = Regex.Replace(text, Environment.NewLine, String.Empty);

      Tokens =
        MarkManaSymbolGroups(
          Tokenizer
            .Split(text)
            .Where(x => x != String.Empty)
            .Where(x => x != " ")
            .Select(CreateToken).ToList());

      _str = text;
    }

    public IEnumerable<Token> AbilityTokens
    {
      get
      {
        var indexOfColon = Tokens.FindIndex(0, token => token.Value == ":");

        if (indexOfColon > 0)
          return Tokens.Skip(indexOfColon + 1);


        return Tokens;
      }
    }


    public int CharacterCount { get { return _str.Length; } }

    public List<Token> Tokens { get; private set; }

    public IEnumerator<Token> GetEnumerator()
    {
      return Tokens.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public override string ToString()
    {
      return _str;
    }

    public string GetTextOnly()
    {
        var sb = new StringBuilder();

        foreach (var token in Tokens)
        {
            if (token is TextToken || token is ImportantTextToken || token is ReminderTextToken)
            {
                sb.Append(token.Value);
                sb.Append(" ");
            }
        }

        return sb.ToString();
    }

    private static Token CreateSpecialToken(string value)
    {
      foreach (var ctor in TokenFactoy)
      {
        var token = ctor(value);
        if (token != null)
          return token;
      }

      return new ImportantTextToken(value);
    }

    private static Token CreateToken(string tokenString)
    {
      var match = Brackets.Match(tokenString);

      return match.Success
        ? CreateSpecialToken(match.Groups[1].Value)
        : new TextToken(tokenString);
    }

    private static List<Token> MarkManaSymbolGroups(List<Token> tokens)
    {
        Token previous = null;
        var marked = new List<Token>();
        bool isReminderText = false;

        foreach (var t in tokens)
        {
            var token = t;

            if ((token is ManaSymbolToken) && (previous is TextToken))
            {
                marked.Add(new ManaSymbolGroupStartToken());
            }

            if ((token is TextToken) && (previous is ManaSymbolToken))
            {
                marked.Add(new ManaSymbolGroupEndToken());
            }

            if (token is TextToken && (isReminderText || previous is ReminderTextOpenToken))
            {
                isReminderText = true;
                token = new ReminderTextToken(token.Value);
            }
            if (token is ReminderTextCloseToken)
            {
                isReminderText = false;
            }

            marked.Add(token);
            previous = token;
        }

        return marked;
    }

    public static implicit operator CardText(string source)
    {
      return new CardText(source);
    }
  }

  public class ConnectingToken : Token
  {
    public ConnectingToken() : base(String.Empty) {}
  }

  public abstract class Token
  {
    protected Token(string value)
    {
      Value = value;
    }

    public string Value { get; private set; }

    public override string ToString()
    {
      return Value;
    }
  }

  public class TextToken : Token
  {
    public TextToken(string value) : base(value) {}
  }

  public class ImportantTextToken : Token
  {
    public ImportantTextToken(string value) : base(value) {}
  }

  public class ReminderTextOpenToken : Token
  {
      public ReminderTextOpenToken() : base(String.Empty) { }
  }

  public class ReminderTextCloseToken : Token
  {
      public ReminderTextCloseToken() : base(String.Empty) { }
  }

  public class ReminderTextToken : Token
  {
      public ReminderTextToken(string value) : base(value) { }
  }

  public class EolToken : Token
  {
    public EolToken() : base("EOL") {}
  }

  public class ManaSymbolGroupStartToken : Token
  {
    public ManaSymbolGroupStartToken() : base(String.Empty) {}
  }

  public class ManaSymbolGroupEndToken : Token
  {
    public ManaSymbolGroupEndToken() : base(String.Empty) {}
  }

  public class ManaSymbolToken : Token
  {
    public ManaSymbolToken(string value) : base(value) {}

    public ImageSource Image { get { return MediaLibrary.GetImage(Value + ".png"); } }
  }
}