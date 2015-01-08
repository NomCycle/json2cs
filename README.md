<h3>JSONCS - Very simple JSON parser in C#</h3>
This quick and simple parser will convert json strings into C# generic dictionaries and lists.

<b>Example JSON and C# code:</b>
```
{
  "integer" : 42,
  "decimal" : 3.14,
  "word" : "why",
	
  "list" : [
    "smells",
    {
      "word" : "ribbit",
      "integer" : 80085
    }
  ]

  "object" : {

    "sentence" : "She sells sea shells by the sea shore",

    "list" : [
      "who",
      "knows",
      "why"
    ]
  }
}
```
```
JSON json = JSON.Parse( str );

int integer = json[ "integer" ];
float floating = json[ "decimal" ];
int converted = json[ "decimal" ];

string word = json[ "word" ];
string element = json[ "list" ][ 0 ];

string sentence = json[ "object" ][ "sentence" ];

```
