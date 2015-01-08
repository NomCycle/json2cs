using System.Collections.Generic;

public class JSON {

	public static JSON Parse ( ref string json, int start = 0 ) {
		return new JSONObject( ParseObject( ref json, ref start ) );
	}

	private static string ParseKey ( ref string json, int i ) {

		bool begin = false;
		string key = "";

		while ( i > 0 ) {

			if ( json[ i ] == '\"' ) {
				begin = true;
				break;
			}

			i--;
		}

		if ( !begin ) 
			return key;

		int end = i;

		for ( i = i - 1; i > 0; i-- ) {
			if ( json[ i ] == '\"' ) {
				i++;
				break;
			}
		}

		key = json.Substring( i, end - i ); 

		return key;
	}

	private static JSON ParseValue ( ref string json, ref int i ) {

		while ( i < json.Length ) {
			switch ( json[ i ] ) {

				case '\"' :
					return new JSONString( ParseString( ref json, ref i ) );

				case '[' :
					return new JSONList( ParseList( ref json, ref i ) );

				case '{' :
					return new JSONObject( ParseObject( ref json, ref i ) );

				default :
					if ( char.IsNumber( json[ i ] ) ) {
						return new JSONNumber( ParseNumber( ref json, ref i ) );
					}
				break;
			}

			i++;
		}

		return null;
	}

	private static string ParseString ( ref string json, ref int i ) {

		string str = "";

		for ( i = i + 1; i < json.Length; i++ ) {

			if ( json[ i ] == '\"' )
				break;

			str += json[ i ];
		}

		return str;
	}

	private static List< JSON > ParseList ( ref string json, ref int i ) {

		List< JSON > values = new List< JSON >();
		int start = i + 1;

		values.Add( ParseValue( ref json, ref start ) );

		for ( i = start; i < json.Length; i++ ) {

			if ( json[ i ] == ']' )
				break;

			if ( json[ i ] == ',' ) {
				values.Add( ParseValue( ref json, ref i ) );
				continue;
			}
		}

		return values;
	}

	private static Dictionary< string, JSON > ParseObject ( ref string json, ref int i ) {

		Dictionary< string, JSON > values = new Dictionary< string, JSON >();

		for ( i = i + 1; i < json.Length; i++ ) {

			if ( json[ i ] == '}' )
				break;

			if ( json[ i ] == ':' ) {

				string key = ParseKey( ref json, i - 1 );
				JSON value = ParseValue( ref json, ref i );
				
				values.Add( key, value );
			}
		}

		return values;
	}

	private static double ParseNumber ( ref string json, ref int i ) {

		while ( i < json.Length ) {

			if ( char.IsNumber( json[ i ] ) )
				break;

			i++;
		}

		string number = "";

		for ( ; i < json.Length; i++ ) {
			if ( !char.IsNumber( json[ i ] ) && json[ i ] != '.' )
				break;
	
			number += json[ i ];
		}

		return double.Parse( number );
	}

	public static implicit operator string( JSON json ) {
		return ( json as JSONString ).value;
	}
	
	public static implicit operator JSON( string str ) {
		return new JSONString( str );
	}
	
	public static implicit operator int( JSON json ) {
		return ( int )( json as JSONNumber ).value;
	}
	
	public static implicit operator JSON( int value ) {
		return new JSONNumber( value );
	}
	
	public static implicit operator float( JSON json ) {
		return ( float )( json as JSONNumber ).value;
	}
	
	public static implicit operator JSON( float value ) {
		return new JSONNumber( value );
	}
	
	public static implicit operator double( JSON json ) {
		return ( json as JSONNumber ).value;
	}
	
	public static implicit operator JSON( double value ) {
		return new JSONNumber( value );
	}
	
	public JSON this[ int i ] {
		get {
			return ( this as JSONList ).values[ i ];
		}
		
		set {
			( this as JSONList ).values[ i ] = value; 
		}
	}
	
	public static implicit operator List< JSON >( JSON json ) {
		return ( json as JSONList ).values;
	}
	
	public static implicit operator JSON( List< JSON > list ) {
		return new JSONList( list );
	}
	
	public JSON this[ string key ] {
		get {
			return ( this as JSONObject ).values[ key ];
		}
		
		set {
			( this as JSONObject ).values[ key ] = value;
		}
	}
	
	public static implicit operator Dictionary< string, JSON >( JSON json ) {
		return ( json as JSONObject ).values;
	}
	
	public static implicit operator JSON( Dictionary< string, JSON > dictionary ) {
		return new JSONObject( dictionary );
	}
}

public class JSONString : JSON {
	
	public JSONString ( string value ) {
		this.value = value;
	}
	
	public string value;
}

public class JSONNumber : JSON {
	
	public JSONNumber ( int value ) {
		this.value = ( double )value;
	}
	
	public JSONNumber ( float value ) {
		this.value = ( double )value;
	}
	
	public JSONNumber ( double value ) {
		this.value = value;
	}

	public static implicit operator string( JSONNumber json ) {
		return ( json as JSONNumber ).value.ToString();
	}

	public static implicit operator JSONNumber( string str ) {
		return new JSONNumber( double.Parse( str ) );
	}
	
	public double value;
}

public class JSONList : JSON {
	
	public JSONList ( List< JSON > values ) {
		this.values = values;
	}
	
	public List< JSON > values = new List< JSON >();
}

public class JSONObject : JSON {
	
	public JSONObject ( Dictionary< string, JSON > values ) {
		this.values = values;
	}
	
	public Dictionary< string, JSON > values = new Dictionary< string, JSON >();

	public void Add( string key, JSON value ) {
		values.Add( key, value );
	}

	public void Remove( string key, JSON value ) {
		values.Remove( key );
	}
}