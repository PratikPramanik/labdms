/**
 * Written by Pratik Pramanik
 * 
**/

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//stuff i added
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public partial class SearchT2 : System.Web.UI.Page
{
    const int DEF_RESULT_SZ = 128;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Retrieve the Query, and URL in case wants to email the search page to friend
        string URLquery = Request.QueryString["T2"];
        string currentURL = Request.Url.ToString();
        
        //on load check if query is avail
        if (URLquery != null && !URLquery.Equals("") && !Page.IsPostBack)
        {
            //failsafes in place... change text in some labels, the lookup procedure
            string query = URLquery;

            if (!query.Equals("") || query != null)
            {
                Label1.Text = "Query Entered: \"" + query + "\"";
                LookupQuery(query);
            }
        }
        else if (Page.IsPostBack)  //redirect with new text
        {
            Response.Redirect("SearchT2.aspx?T2="
                          + Server.UrlEncode(CommonSearchTextBox.Text));
        }
        else  //no text in querybox
            Results.Text = "Please enter a query in the box above";
    }

    protected void T2_Click(object sender, EventArgs e)
    {
        //Redirect to search page
        Response.Redirect("SearchT2.aspx?T2="
                          + Server.UrlEncode(CommonSearchTextBox.Text));
    }

    /* LookupQuery
     * 
     * Reads in a Serialized HashTable and finds keywords matching
     * the query given.
     */
    protected void LookupQuery(string query)
    {
        Hashtable wordmap = new Hashtable();
        //delimination symbols
        char[] delimCh =  { ' ', ',', '.', ':', '\t', '_', '\n', 
                            '!','@','#','$','%','^','&','*','(',
                            ')','[',']',';','\'','{','}','\"',
                            '<','>','\\','/','+','-','`','\r',
                            '~'};

        #region Database grabbing for data
        //Grabbing database and making a table of ID->Desc and Syn
        #endregion

        //Read in HashTable file index.dat
        #region Reading index.dat
        FileStream fs = new FileStream(Server.MapPath("~/index.dat"), 
                                        FileMode.Open, 
                                        FileAccess.Read);

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            wordmap = (Hashtable)bf.Deserialize(fs);
        }
        finally
        {
            fs.Close();
        }
        //need a catch?
        #endregion

        //Store the query terms as structs
        #region Analyzing the query terms
        string[] terms = query.Split(delimCh);
        Term[] termArr = new Term[terms.Length];
        for(int i = 0; i < terms.Length; i++)
        {
            //check for flags //NOT YET CHANGE SPLIT WHEN UPDATED
            //flags "code:" "title:"
            //forget wild cards for now

            //else just store
            termArr[i] = new Term(terms[i].ToLower());
        }
        #endregion

        Result[] resultArr = new Result[DEF_RESULT_SZ];
        List<Result> resultList = new List<Result>();

        #region Finding the terms on the wordmap
        //Check each term individually
        foreach (Term q in termArr)
        {
            //skip stopwords
            if (isStopword(q.GetWord()) || q.isOmitted())
            {
                q.OmitIt();
                continue;
            }

            //Key found in the Hashtable
            if (wordmap.ContainsKey(q.GetWord()) && !q.isFlagged())
            {
                string[] locations = (string[])wordmap[q.GetWord()];

                //check if found already
                foreach (string loc in locations)
                {
                    if (loc != null)
                    {
                        //convert string into Result
                        string[] tempSIT = loc.Split(':');
                        Result temp = new Result(Convert.ToInt32(tempSIT[0]),
                                                 Convert.ToInt32(tempSIT[1]),
                                                 tempSIT[2]);

                        //already on list? add score
                        if (resultList.Contains(temp))
                        {
                            resultList[resultList.IndexOf(temp)].AddScore(temp.GetScore());
                        }
                        else //if not add a fresh COPY to the List
                        {
                            resultList.Add(new Result(temp));
                        }
                    }
                    else
                        break;
                }
            }
            //Key has a special flag to be handled
            else if (wordmap.ContainsKey(q.GetWord()) && q.isFlagged())
            {
                //place holder
                int five = 2 + 3;
            }
            //Query term not found on 
            else
            {
                q.Found(false);
            }
        }
        #endregion

        #region Working with the results.
        //sort through the result list and display to user. 
        //no results tell user there are none
        if (resultList.Count != 0)
        {
            resultList.Sort();
        }

        //ObjectDataSource ods = 
        //    new ObjectDataSource
        //        ("AdsDataComponentTableAdapters.AdsDataAdapter", "GetAdByID");

        //ObjectDataSource1.SelectParameters.Add("ID", 1);

        Results.Text = "";
        foreach (Result r in resultList)
        {
            Results.Text += "<b>" + r.GetTitle() + "</b><br />" + 
                            //"<em>Score: " + r.GetScore() + "</em><br />" + 
                            //"Desc: " + 
                            "<a href=" + r.GetURL() + ">" + r.GetURL() + "</a>" + 
                            "<br /><br />";
        }
        #endregion
    }

    #region Helper Methods
    /* Checks if passed in word "s" is a stopword of the English language*/
    protected bool isStopword(string s)
    {
        #region BIG LIST OF WORDS
        string[] stopwords = {"a", "about", "above", "above", "across", "after", "afterwards", 
        "again", "against", "all", "almost", "alone", "along", "already", "also","although",
        "always","am","among", "amongst", "amoungst", "amount",  "an", "and", "another", "any",
        "anyhow","anyone","anything","anyway", "anywhere", "are", "around", "as",  "at", "back",
        "be","became", "because","become","becomes", "becoming", "been", "before", "beforehand", 
        "behind", "being", "below", "beside", "besides", "between", "beyond", "bill", "both", 
        "bottom","but", "by", "call", "can", "cannot", "cant", "co", "con", "could", "couldnt", 
        "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", 
        "eight", "either", "eleven","else", "elsewhere", "empty", "enough", "etc", "even", "ever", 
        "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", 
        "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", 
        "four", "from", "front", "full", "further", "get", "give", "go", "had", "has", "hasnt", 
        "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", 
        "herself", "him", "himself", "his", "how", "however", "hundred", "ie", "if", "in", "inc", 
        "indeed", "interest", "into", "is", "it", "its", "itself", "keep", "last", "latter", 
        "latterly", "least", "less", "ltd", "made", "many", "may", "me", "meanwhile", "might", 
        "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my", 
        "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "no", 
        "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "of", "off", 
        "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", 
        "our", "ours", "ourselves", "out", "over", "own","part", "per", "perhaps", "please", 
        "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", 
        "several", "she", "should", "show", "side", "since", "sincere", "six", "sixty", "so", 
        "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere", "still", 
        "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", 
        "then", "thence", "there", "thereafter", "thereby", "therefore", "therein", "thereupon", 
        "these", "they", "thick", "thin", "third", "this", "those", "though", "three", "through", 
        "throughout", "thru", "thus", "to", "together", "too", "top", "toward", "towards", "twelve", 
        "twenty", "two", "un", "under", "until", "up", "upon", "us", "very", "via", "was", "we", 
        "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", 
        "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", 
        "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", 
        "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves"};
        #endregion

        //run s through array
        for (int n = 0; n < stopwords.Length; n++)
        { if (s.Equals(stopwords[n])) { return true; } }
        return false;
    }

    /* Formats words into a standarized format */
    protected string FormatForm(int score, int ID, string name)
    {
        return score + ":" + ID + ":" + name;
    }

    /* Unformats from a standard format to a string array */
    protected string[] UnFormatForm(string formatted)
    {
        string [] split = formatted.Split(':');

        //if it is an unexpected length, return null
        if (split.Length != 3) { return null; }
        
        return split;
    }
    #endregion
}

/* Author: Pratik Pramanik */
struct Result : IComparable<Result>
{ 
    int ID, Score;
    String TitleCode; 

    //regular contructor
    public Result(int score, int id, string titlecode)
    {
        ID = Convert.ToInt32(id);
        TitleCode = titlecode;
        Score = score;
    }
    //copy constructor
    public Result(Result s)
    {
        ID = s.GetID();
        Score = s.GetScore();
        TitleCode = s.GetTitle();
    }

    public void AddScore(int score) { Score += score; }
    public int CompareTo(Result o)
    {
        if (Score < o.GetScore())
            return 1;
        if (Score == o.GetScore())
            return 0;
        else
            return -1;
    }
    public bool Equals(Result o)
    {
        if (o.GetTitle().Equals(TitleCode) || o.GetID() == ID)
            return true;
        else
            return false;
    }
    public int GetID()       { return ID; }
    public int GetScore()    { return Score; }
    public string GetTitle() { return TitleCode; }
    public string GetURL()   { return "http://labdms/ShowAd.aspx?id=" + ID; }
}

/* Author: Pratik Pramanik */
struct Term
{
    //basic level
    bool Omit;
    string Word;

    //flags
    bool Title_only;
    bool Code_only;
    bool Wild_card;

    //conditions
    bool Not_found;

    //"default"
    public Term(string word)
    {
        Word = word;
        Omit = false;
        Title_only = false;
        Code_only = false;
        Not_found = false;
        Wild_card = false;
    }

    //for when flags are initiated
    public Term(string word, bool title, bool code, bool wild)
    {
        Word = word;
        Omit = false;
        Title_only = title;
        Code_only = code;
        Wild_card = wild;
        Not_found = false;
    }

    public void OmitIt()       { Omit = true; }
    public string GetWord()    { return Word; }
    public bool isOmitted()    { return Omit; }
    public bool isNotIndexed() { return Not_found; }
    public void Found(bool b)  { Not_found = b; }
    public bool isFlagged()    { return Title_only | Code_only | Wild_card; }
}

struct Description
{
    string Desc;
    string Syn1;
    string Syn2;
    string Syn3;
    string Syn4;
    string Syn5;
}
//String {score}:ID:formattedname