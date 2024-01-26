<Query Kind="Statements" />

var search = new Regex (@"httpclient", RegexOptions.IgnoreCase);

var queries =
	from query in Util.GetMyQueries().Concat (Util.GetSamples()).AsParallel().AsOrdered()
	where query.IsCSharp || query.IsSQL
	let matches = search.Matches (query.Text)
	where matches.Count > 0 || search.IsMatch (query.Name)
	group new { Query = query.OpenLink, Matches = query.FormatMatches (matches) } by query.Location;

foreach (var item in queries)
	item.ToArray().Dump (item.Key);
