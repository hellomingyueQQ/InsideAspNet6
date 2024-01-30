<Query Kind="Program">
  <Namespace>System.Text.Json</Namespace>
</Query>

class Program
{
	static void Main()
	{

		string json = """
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="Newtonsoft.Json" version="13.0.1" targetFramework="net472" />
  <package id="EntityFramework" version="6.4.4" targetFramework="net472" />
  <package id="NLog" version="4.7.12" targetFramework="net472" />
</packages>

""";

		File.WriteAllText("packages.config", json);
		List<Package> packages = new List<Package>();

		// 加载 XML 文件
		XmlDocument doc = new XmlDocument();
		doc.Load("packages.config");

		// 获取所有的 <package> 元素
		XmlNodeList packageNodes = doc.SelectNodes("/packages/package");
		foreach (XmlNode packageNode in packageNodes)
		{
			// 读取 package 元素的属性值
			string id = packageNode.Attributes["id"].Value;
			string version = packageNode.Attributes["version"].Value;
			string targetFramework = packageNode.Attributes["targetFramework"].Value;

			// 创建 Package 对象并添加到集合中
			Package package = new Package(id, version, targetFramework);
			packages.Add(package);
		}

		// 打印集合中的 Package 信息
		foreach (Package package in packages)
		{
			Console.WriteLine($"ID: {package.Id}, Version: {package.Version}, Target Framework: {package.TargetFramework}");
		}
	}
}

class Package
{
	public string Id { get; }
	public string Version { get; }
	public string TargetFramework { get; }

	public Package(string id, string version, string targetFramework)
	{
		Id = id;
		Version = version;
		TargetFramework = targetFramework;
	}
}


