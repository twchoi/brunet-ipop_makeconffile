using System;
using System.IO;
using System.Net;
using System.Xml;

namespace xmlGenerator
{
    class xmlMaker
    {
        static void Main()
        {
			string menuSel;
			bool running;
			
			running = true;
			while(running)
			{
				Console.WriteLine("");
				Console.WriteLine("=============== M E N U ================");
				Console.WriteLine("1. Generate Brunet configuration file");
				Console.WriteLine("2. Generate IPOP configuration file");
				Console.WriteLine("3. Generate DHCP configuration file");
				Console.WriteLine("11. Read XML file");
				Console.WriteLine("99. Exit");
				Console.WriteLine("========================================");
				Console.WriteLine("Type number : ");
				menuSel = Console.ReadLine();
				switch(menuSel)
				{
					case "1" :
						genBrunetConf();
						break;

					case "2" :
						genIPOPConf();
						break;

					case "3" :
						genDHCPConf();
						break;

					case "11" :
						ReadXMLFile();
						break;

					case "99" : 
						running = false;
						break;

					default :
						Console.WriteLine("Type appropriate menu number");
						break;
				}
			}
        }
/* 
** Generate Brunet Configuration file
*/
		static bool genBrunetConf()
		{
			XmlTextWriter textWriter;
			string confPath;
			string inputString;
 
			Console.WriteLine("Type Brunet configuration file path and name(for default value, press enter. default:node.config)");
			confPath = Console.ReadLine();
			if(confPath == ""){  //default value
				confPath = "node.config";
			}

			try{
				textWriter = new XmlTextWriter(confPath, null);
			}catch(Exception x){
				Console.WriteLine("Fail to create xml file. Check file path and name");
				return false;
			}
			
            textWriter.WriteStartDocument();
            textWriter.WriteComment("Brunet Configuration file");

			textWriter.WriteStartElement("NodeConfig");

			WriteElementValue("BrunetNamespace", null, "default_Brunet", textWriter, ValidCheck.NoCheck);

			textWriter.WriteStartElement("RemoteTAs");
			do{
				Console.WriteLine("Enter well known end points(ex: \"udp://71.122.33.252:12342\"). If you have no well known end points, press enter");
				inputString = Console.ReadLine();
				if (inputString != null){
					if(inputString != ""){
						if(isValidRemoteTA(inputString)){
							textWriter.WriteStartElement("Transport");
							textWriter.WriteString("brunet." + inputString);
							textWriter.WriteEndElement();
						}   //if input is not valid, type one more time
					}
					else{   //user does not want to make RemoteTA list
						break;
					}
				}
			}while(inputString != null && inputString != "");  
			textWriter.WriteEndElement();

			textWriter.WriteStartElement("EdgeListeners");
			do{
				Console.WriteLine("Type local end point(EdgeListener)type of transport logic(udp or tcp). If you don't want to deploy EdgeListener, press enter");
				inputString = Console.ReadLine();
				if (inputString != null){
					if(inputString != ""){   //user typed no character
						if(inputString == "tcp" || inputString == "udp"){
							textWriter.WriteStartElement("EdgeListener");
							textWriter.WriteAttributeString("type",inputString);
							WriteElementValue("port",null,"12342",textWriter,ValidCheck.PortCheck);
							textWriter.WriteEndElement();   //for EdgeListener element
						}
						else{
							continue;  //input string is not valid. 
						}
					}
					else{
						break;  //user press enter
					}
				}
			}while(inputString != null && inputString != "");

			textWriter.WriteEndElement();   //for EdgeListeners

 			textWriter.WriteStartElement("XmlRpcManager");
			do{
				Console.WriteLine("Would you use XmlRpcManager(type \"true\" or \"false\" default=true) ?");
				inputString = Console.ReadLine();
				if (inputString != null){
					if(inputString != ""){
						if(inputString == "true" || inputString == "false"){
							WriteElementValue("Enabled", inputString, null, textWriter, ValidCheck.NoCheck);
						}
						else{
							Console.WriteLine("Type only \"true\" or \"false\"");
							continue;
						}
					}
					else{   //user press enter --> default string
						WriteElementValue("Enabled", "true", null, textWriter,ValidCheck.NoCheck);
					}
					WriteElementValue("port", "10000", null, textWriter, ValidCheck.NoCheck);
					break;
				}
				else{
					Console.WriteLine("
				}
			}while(true);
			textWriter.WriteEndElement();	//for XmlRpcManager

			textWriter.WriteStartElement("RpcDht");
			do{
				Console.WriteLine("Would you use RpcDht(type \"true\" or \"false\" default=true) ?");
				inputString = Console.ReadLine();
				if (inputString != null){
					if(inputString != ""){
						if(inputString == "true" || inputString == "false"){
							WriteElementValue("Enabled", inputString, null, textWriter,ValidCheck.NoCheck);
						}
						else{
							Console.WriteLine("Type only \"true\" or \"false\"");
							continue;
						}
					}
					else{  //user press enter --> default string
						WriteElementValue("Enabled", "true", null, textWriter,ValidCheck.NoCheck);
					}
					WriteElementValue("port", "64221", null, textWriter, ValidCheck.NoCheck);
					break;
				}
				else{
					Console.WriteLine("
				}
			}while(true);
			textWriter.WriteEndElement();	//for RpcDht
			
			textWriter.WriteEndElement();	// for NodeConfig
            textWriter.WriteEndDocument();
            textWriter.Close();
			return true;

		}

/* 
*Generate IPOP Configuration file
*/	
		static bool genIPOPConf()
		{
			XmlTextWriter textWriter;
			string confPath;

			Console.WriteLine("Type IPOP configuration file path and name(for default value, press enter. default:ipop.config)");
			confPath = Console.ReadLine();
			if(confPath == ""){
				confPath = "ipop.config";
			}
			
			try{
				textWriter = new XmlTextWriter(confPath, null);
			}catch(Exception x){
				Console.WriteLine("Fail to create xml file. Check file path and name");
				return false;
			}
			
			textWriter.WriteStartDocument();
			textWriter.WriteComment("Ipop Configuration file");
		
			textWriter.WriteStartElement("IpopConfig");

			
			WriteElementValue("IpopNamespace", null, "IpopTest_default", textWriter,ValidCheck.NoCheck);
			WriteElementValue("VirtualNetworkDevice", null, "tapipop", textWriter, ValidCheck.NoCheck);
			
			textWriter.WriteStartElement("AddressData");
			WriteElementValue("Hostname", null, "default_hostname", textWriter, ValidCheck.NoCheck);
			textWriter.WriteEndElement(); //AddressData
			
			textWriter.WriteEndElement();	// for IpopConfig
			textWriter.WriteEndDocument();
			textWriter.Close();
			return true;
		
		}

/* 
*Generate DHCP Configuration file
*/
		static bool genDHCPConf()
		{
			XmlTextWriter textWriter;
			string inputString;
			string confPath;

			Console.WriteLine("Type DHCP configuration file path and name(for default value, press enter. default:DHCPServer.Config)");
			confPath = Console.ReadLine();
			if(confPath == ""){
				confPath = "DHCPServer.Config";
			}
			
			try{
				textWriter = new XmlTextWriter(confPath, null);
			}catch(Exception x){
				Console.WriteLine("Fail to create xml file. Check file path and name");
				return false;
			}
			
			textWriter.WriteStartDocument();
			textWriter.WriteComment("DHCP Configuration file");
		
			textWriter.WriteStartElement("DHCPServerConfig");
			
			WriteElementValue("Namespace", null, "IpopTest_default", textWriter, ValidCheck.NoCheck);
			WriteElementValue("netmask", null, "255.255.0.0", textWriter, ValidCheck.IPCheck);

			textWriter.WriteStartElement("pool");
			WriteElementValue("lower", null, "10.250.0.0", textWriter, ValidCheck.IPCheck);
			WriteElementValue("upper", null, "10.250.255.255", textWriter, ValidCheck.IPCheck);
			textWriter.WriteEndElement();

			textWriter.WriteStartElement("DHCPReservedIPs");
			do{
				Console.WriteLine("Do you want to reserve any IPs(type \"yes\" or \"no\" ,default=\"yes\")?");
				inputString = Console.ReadLine();
				if (inputString != null){
					if((inputString == "yes") || (inputString=="")){
						textWriter.WriteStartElement("DHCPReservedIP");
						WriteElementValue("ip", null, "0.0.0.1", textWriter, ValidCheck.IPCheck);
						WriteElementValue("mask", null, "0.0.0.255", textWriter, ValidCheck.IPCheck);
						textWriter.WriteEndElement();   //for DHCPReservedIP element
					}
					else if(inputString == "no"){
						break;
					}
					else{
						Console.WriteLine("Type only \"yes\" or \"no\"");
						continue;
					}
				}
			}while(inputString != null && inputString != "no");
			textWriter.WriteEndElement(); //for DHCPReservedIPs
			
			WriteElementValue("leasetime", null, "3600", textWriter, ValidCheck.NoCheck);
					
			textWriter.WriteEndElement();	// for DHCPServerConfig
			textWriter.WriteEndDocument();
			textWriter.Close();
			return true;
		}

/* 
**Create an element and fill the element value.
**string element : the name of element to be added
**string value : The value of element. If user types the value, this variable should be null
**string defaultString : The default value for the element. If "value" is null(The user will type the value), default string should exist.
**XmlTextWriter TargetXML : XML instance which we should create an element and write a value.
**ValidCheck type : Desired type of validation check for the input value. refer to ValidCheck enumerator
*/
		static void WriteElementValue(string element, string value, string defaultString, XmlTextWriter TargetXML, ValidCheck type)
		{
			string inputString;
			
			TargetXML.WriteStartElement(element);
			if(value == null){ //if value==null, defaultString always exists.
				while(true){
					Console.WriteLine("Type \"" + element + "\" value(default=" +defaultString+") : ");
					inputString = Console.ReadLine();
					if(inputString == ""){
						TargetXML.WriteString(defaultString);
						break;
					}
					else{
						if(type== ValidCheck.IPCheck){   //IP valid check
							if(isValidIP(inputString)){
								TargetXML.WriteString(inputString);
								break;
							}
							else{
								Console.WriteLine("Type valid IP address");
								continue;
							}
						}
						else if(type==ValidCheck.PortCheck){   //port number valid check 
							if(isValidPort(inputString)){
								TargetXML.WriteString(inputString);
								break;
							}
							else{
								Console.WriteLine("Type valid Port Number");
								continue;
							}
						}
						else{   //no valid check needed
							TargetXML.WriteString(inputString);  
							break;
						}
					}
				}
			}
			else{
				TargetXML.WriteString(value);
			}
			TargetXML.WriteEndElement();
		}
/*
** Read XML file and show the result on the console.
*/
		static bool ReadXMLFile()
		{
			string xmlFilePath;
			XmlTextReader reader;
			Console.WriteLine("Enter xml file path and name");
			xmlFilePath = Console.ReadLine();

            XmlDocument doc = new XmlDocument();

			try{
	    		reader = new XmlTextReader(xmlFilePath);
			}catch(FileNotFoundException x){
				Console.WriteLine("Fail to open xml file. Check file path and name");
				return false;
			}
            reader.Read();
            doc.Load(reader);
            doc.Save(Console.Out);
			reader.Close();

			return true;
		}
/*
** Check whether the input IP string is valid or not
*/
		static bool isValidIP(string ipAddr)
		{
			bool isValid = true;
		
			IPAddress ip;

			if(ipAddr == null){
		        isValid = false;
		    }
			else{
				isValid = IPAddress.TryParse(ipAddr,out ip);
			}

			return isValid;
		}

/*
** Check whether the input Port value is valid or not
*/
		static bool isValidPort(string portNumber)
		{
			uint port;
			try{
			port = Convert.ToUInt32(portNumber);
			}catch(Exception x){
				Console.WriteLine("Fail to convert port number string to int value");
				return false;
			}

			if((port >= 1024) && (port <= 65536)){
				return true;
			}
			else{
				return false;
			}
		}

/*
** Check whether the input RemoteTA value is valid or not
** It will check the start of the string("udp://" or "tcp://")
** It will check IP validity and Port number validity
*/
		static bool isValidRemoteTA(string address)
		{
			int endIP;
			string IPaddr; //4 
			string PortNum;

			if(!((address.Trim().StartsWith("udp://")) || (address.Trim().StartsWith("tcp://")))){
				Console.WriteLine("RemoteTA string starts with invalid value. It should start with \"tcp://\" or \"udp://\"");
				return false;
			}

			endIP = address.IndexOf(":",6);  //6 = to ignore "udp://" or "tcp://"
			if(endIP == -1){
				Console.WriteLine("IP and port number deliminator is not found.");
				return false;
			}

			try{
				IPaddr = address.Substring(6, endIP-6);   //endIP-6 = end of IP string
				PortNum = address.Substring(endIP + 1); // 1 = ":"
			}catch(Exception x){
				Console.WriteLine("Exception while copying IP or Port number from RemoteTA input");
				return false;
			}

			if(!isValidIP(IPaddr)){
				Console.WriteLine("Invalid IP value in RemoteTA");
				return false;				
			}
			if(!isValidPort(PortNum)){
				Console.WriteLine("Invalid Port number in RemoteTA");
				return false;				
			}

			return true;
		}
/*
** Enumerator value which will be used for validity check option
*/
	    protected enum ValidCheck : int
	    {
			NoCheck = 1,
			IPCheck = 2, 
			PortCheck = 3 
	    }
    }
}
