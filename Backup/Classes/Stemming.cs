using System;
using System.Collections;

namespace SearchO
{
	public class Stemming
	{
		ArrayList DocToken;
		private char[] each_word;
		private int i_end, /* offset to end of stemmed word */
			j, k;
		public Stemming(ArrayList unStemmedToken)
		{
			DocToken = unStemmedToken;
			//
			// TODO: Add constructor logic here
			//
		}
		public override string ToString() 
		{
			return new String(each_word,0,i_end);
		}
		/* cons(i) is true <=> each_word[i] is a consonant. */
		private bool cons(int i) 
		{
			switch (each_word[i]) 
			{
				case 'a': case 'e': case 'i': case 'o': case 'u': return false;
				case 'y': return (i==0) ? true : !cons(i-1);
				default: return true;
			}
		}

		private int m() 
		{
			int n = 0;
			int i = 0;
			while(true) 
			{
				if (i > j) return n;
				if (! cons(i)) break; i++;
			}
			i++;
			while(true) 
			{
				while(true) 
				{
					if (i > j) return n;
					if (cons(i)) break;
					i++;
				}
				i++;
				n++;
				while(true) 
				{
					if (i > j) return n;
					if (! cons(i)) break;
					i++;
				}
				i++;
			}
		}

		/* vowelinstem() is true <=> 0,...j contains a vowel */
		private bool vowelinstem() 
		{
			int i;
			for (i = 0; i <= j; i++)
				if (! cons(i))
					return true;
			return false;
		}
		/* doublec(j) is true <=> j,(j-1) contain a double consonant. */
		private bool doublec(int j) 
		{
			if (j < 1)
				return false;
			if (each_word[j] != each_word[j-1])
				return false;
			return cons(j);
		}
		/* cvc(i) is true <=> i-2,i-1,i has the form consonant - vowel - consonant
		   and also if the second c is not w,x or y. this is used when trying to
		   restore an e at the end of a short word. e.g.

			  cav(e), lov(e), hop(e), crim(e), but
			  snow, box, tray.

		*/
		private bool cvc(int i) 
		{
			if (i < 2 || !cons(i) || cons(i-1) || !cons(i-2))
				return false;
			int ch = each_word[i];
			if (ch == 'w' || ch == 'x' || ch == 'y')
				return false;
			return true;
		}

		private bool ends(String s) 
		{
			int l = s.Length;
			int o = k-l+1;
			if (o < 0)
				return false;
			char[] sc = s.ToCharArray();
			for (int i = 0; i < l; i++)
				if (each_word[o+i] != sc[i])
					return false;
			j = k-l;
			return true;
		}
		/* setto(s) sets (j+1),...k to the characters in the string s, readjusting
		   k. */
		private void setto(String s) 
		{
			int l = s.Length;
			int o = j+1;
			char[] sc = s.ToCharArray();
			for (int i = 0; i < l; i++)
				each_word[o+i] = sc[i];
			k = j+l;
		}

		/* r(s) is used further down. */
		private void r(String s) 
		{
			if (m() > 0)
				setto(s);
		}
		/* step1() gets rid of plurals and -ed or -ing. e.g.
			   caresses  ->  caress
			   ponies    ->  poni
			   ties      ->  ti
			   caress    ->  caress
			   cats      ->  cat

			   feed      ->  feed
			   agreed    ->  agree
			   disabled  ->  disable

			   matting   ->  mat
			   mating    ->  mate
			   meeting   ->  meet
			   milling   ->  mill
			   messing   ->  mess

			   meetings  ->  meet

		*/

		private void step1() 
		{
			if (each_word[k] == 's') 
			{
				if (ends("sses"))
					k -= 2;
				else if (ends("ies"))
					setto("i");
				else if (each_word[k-1] != 's')
					k--;
			}
			if (ends("eed")) 
			{
				if (m() > 0)
					k--;
			} 
			else if ((ends("ed") || ends("ing")) && vowelinstem()) 
			{
				k = j;
				if (ends("at"))
					setto("ate");
				else if (ends("bl"))
					setto("ble");
				else if (ends("iz"))
					setto("ize");
				else if (doublec(k)) 
				{
					k--;
					int ch = each_word[k];
					if (ch == 'l' || ch == 's' || ch == 'z')
						k++;
				}
				else if (m() == 1 && cvc(k)) setto("e");
			}
		}

		/* step2() turns terminal y to i when there is another vowel in the stem. */
		private void step2() 
		{
			if (ends("y") && vowelinstem())
				each_word[k] = 'i';
		}

		/* step3() maps double suffices to single ones. so -ization ( = -ize plus
		   -ation) maps to -ize etc. note that the string before the suffix must give
		   m() > 0. */
		private void step3() 
		{
			if (k == 0)
				return;
			
			switch (each_word[k-1]) 
			{
				case 'a':
					if (ends("ational")) { r("ate"); break; }
					if (ends("tional")) { r("tion"); break; }
					break;
				case 'c':
					if (ends("enci")) { r("ence"); break; }
					if (ends("anci")) { r("ance"); break; }
					break;
				case 'e':
					if (ends("izer")) { r("ize"); break; }
					break;
				case 'l':
					if (ends("bli")) { r("ble"); break; }
					if (ends("alli")) { r("al"); break; }
					if (ends("entli")) { r("ent"); break; }
					if (ends("eli")) { r("e"); break; }
					if (ends("ousli")) { r("ous"); break; }
					break;
				case 'o':
					if (ends("ization")) { r("ize"); break; }
					if (ends("ation")) { r("ate"); break; }
					if (ends("ator")) { r("ate"); break; }
					break;
				case 's':
					if (ends("alism")) { r("al"); break; }
					if (ends("iveness")) { r("ive"); break; }
					if (ends("fulness")) { r("ful"); break; }
					if (ends("ousness")) { r("ous"); break; }
					break;
				case 't':
					if (ends("aliti")) { r("al"); break; }
					if (ends("iviti")) { r("ive"); break; }
					if (ends("biliti")) { r("ble"); break; }
					break;
				case 'g':
					if (ends("logi")) { r("log"); break; }
					break;
				default :
					break;
			}
		}

		/* step4() deals with -ic-, -full, -ness etc. similar strategy to step3. */
		private void step4() 
		{
			switch (each_word[k]) 
			{
				case 'e':
					if (ends("icate")) { r("ic"); break; }
					if (ends("ative")) { r(""); break; }
					if (ends("alize")) { r("al"); break; }
					break;
				case 'i':
					if (ends("iciti")) { r("ic"); break; }
					break;
				case 'l':
					if (ends("ical")) { r("ic"); break; }
					if (ends("ful")) { r(""); break; }
					break;
				case 's':
					if (ends("ness")) { r(""); break; }
					break;
			}
		}

		/* step5() takes off -ant, -ence etc., in context <c>vcvc<v>. */
		private void step5() 
		{
			if (k == 0)
				return;

			switch ( each_word[k-1] ) 
			{
				case 'a':
					if (ends("al")) break; return;
				case 'c':
					if (ends("ance")) break;
					if (ends("ence")) break; return;
				case 'e':
					if (ends("er")) break; return;
				case 'i':
					if (ends("ic")) break; return;
				case 'l':
					if (ends("able")) break;
					if (ends("ible")) break; return;
				case 'n':
					if (ends("ant")) break;
					if (ends("ement")) break;
					if (ends("ment")) break;
					/* element etc. not stripped before the m */
					if (ends("ent")) break; return;
				case 'o':
					if (ends("ion") && j >= 0 && (each_word[j] == 's' || each_word[j] == 't')) break;
					
					if (ends("ou")) break; return;
					/* takes care of -ous */
				case 's':
					if (ends("ism")) break; return;
				case 't':
					if (ends("ate")) break;
					if (ends("iti")) break; return;
				case 'u':
					if (ends("ous")) break; return;
				case 'v':
					if (ends("ive")) break; return;
				case 'z':
					if (ends("ize")) break; return;
				default:
					return;
			}
			if (m() > 1)
				k = j;
		}

		/* step6() removes a final -e if m() > 1. */
		private void step6() 
		{
			j = k;
			
			if (each_word[k] == 'e') 
			{
				int a = m();
				if (a > 1 || a == 1 && !cvc(k-1))
					k--;
			}
			if (each_word[k] == 'l' && doublec(k) && m() > 1)
				k--;
		}
		public ArrayList Stem()
		{
			ArrayList stemmedTokens = new ArrayList();
			foreach(string s in DocToken)
			{
				each_word = s.ToCharArray(); 
				int sizeofchar = each_word.Length-1;
				k = sizeofchar ;
				if (k > 1) 
				{
					step1();
					step2();
					step3();
					step4();
					step5();
					step6();
				}
				i_end = k+1;
				stemmedTokens.Add(this.ToString());
				sizeofchar = 0;
			}
			return stemmedTokens;
		}
	}
}
