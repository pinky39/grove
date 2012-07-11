namespace Grove.Core.Details.Cards
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;
  using System.Windows.Media;
  using Ui;

  public class CardText : IEnumerable<Token>
  {
    private static readonly Regex Brackets = new Regex("{(.*)}", RegexOptions.Compiled);

    private static readonly List<Func<string, Token>> TokenFactoy = new List<Func<string, Token>>
      {
        token => token == "EOL" ? new EolToken() : null,
        token =>
          {
            token = token.ToLowerInvariant();

            if (token == "w" || token == "u" || token == "b" || token == "r" || token == "g" || token == "t" ||
              token == "1" || token == "2" || token == "3" || token == "4" || token == "5" || token == "6" ||
                token == "7" || token == "8")
            {
              return new ManaSymbolToken(token);
            }
            return null;
          }
      };

    private static readonly Regex Tokenizer = new Regex("( )|({[^}]*})", RegexOptions.Compiled);

    public CardText(string text)
    {
      if (String.IsNullOrEmpty(text))
      {
        Tokens = new List<Token> {};
        Original = String.Empty;
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

      Original = text;
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

    public int CharacterCount { get { return Original.Count(); } }

    public string Original { get; private set; }
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
      return Original;
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

      foreach (var token in tokens)
      {
        if ((token is ManaSymbolToken) && (previous is TextToken))
        {
          marked.Add(new ManaSymbolGroupStartToken());
        }

        if ((token is TextToken) && (previous is ManaSymbolToken))
        {
          marked.Add(new ManaSymbolGroupEndToken());
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