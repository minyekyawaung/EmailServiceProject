

namespace EmailService;
public static partial class SQLExtensions
{
    private static string[] sqloperators = { "=", ">", "<", ">=", "<=", "<>", "!=", "between", "like" };
    private static string[] sqllogical = { "and", "or" };
    public static string isParameterized(this string where, bool justread = false)
    {
        var logics = where.ToLower().Split(sqllogical, StringSplitOptions.None);
        foreach (var logic in logics)
        {
            var conditions = logic.Split(sqloperators, StringSplitOptions.None);
            bool isnotvalid = false;

            if (justread)
            {
                //this is for join query
                if (conditions.Length == 2) if (!conditions[1].Replace(" ", "").StartsWith("@") && !conditions[1].Contains(".")) isnotvalid = true;
            }
            else
            {
                if (conditions.Length == 2) if (!conditions[1].Replace(" ", "").StartsWith("@")) isnotvalid = true;
            }
            if (conditions.Length == 1) if (!conditions[0].Replace(" ", "").StartsWith("@")) isnotvalid = true;

            if (isnotvalid) throw new Exception("Where clause has direct assigned values, must use parameter to prevent SQL injection");


        }
        return where;
    }

    //getting extra value from previous query results by tag name and file name
    public static void setValues(this Dictionary<string, object> paras, Dictionary<string, Response> responses)
    {

        foreach (var p in paras)
        {
            if (p.Value.GetType() == typeof(string) && p.Value.ToString().Contains(".") && p.Value.ToString().StartsWith("@"))
            {
                var e = p.Value.ToString().Split(".");
                var a = responses.Where(response => response.Key == e[0].Substring(1)).FirstOrDefault().Value;
                var v = a.rows[0].Where(d => d.Key.ToLower() == e[1].ToLower()).FirstOrDefault().Value;
                paras[p.Key] = v;
            }

        }
    }

}

