<Query Kind="Statements">
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>


var table = new Table(noBorders: true, cellVerticalAlign: "middle");
table.Rows.Add(new TextBox() { Text = "http://localhost:5000/greeting/greet" }, new Button("Click", b =>
{
	Task.Run(async() =>
	{

		using (HttpClient client = new HttpClient())
		{
			string text = await client.GetStringAsync("http://localhost:5000/greeting/greet");
			text.Dump();
		}
	});
}));

table.Dump();    
