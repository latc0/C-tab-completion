protected override bool ProcessTabKey(bool forward)
{
	if (pathBox.Focus() && pathBox.Text.StartsWith("/"))
	{
		string[] items = pathBox.Text.Split('/');
		string firstChars = items[items.Length - 1];//user-entered chars
		string tempFullPath = pathBox.Text.Remove(pathBox.Text.Length - firstChars.Length - 1);//full path to look for completions in
		string[] dirs = phone.GetDirectories(tempFullPath);//list of dirs in searchable path
		if (dirs.Length == 1)
			pathBox.Text = tempFullPath + "/" + dirs[0] + "/";//if only one entry, complete to it
		else
		{
			if (firstChars != "")//check there's something entered
			{
				char[] letters = firstChars.ToCharArray();//split entered string to chars
				ArrayList possibles = new ArrayList();
				foreach (string dir in dirs)//possible dirs in working dir
				{
					if (dir.Length >= letters.Length)//check search string is longer or equal to length of test dir, otherwise can't be a match
					{
						if (dir.StartsWith(firstChars))
						{
							if (!possibles.Contains(dir))
								possibles.Add(dir);
						}
					}
				}
				string newText = pathBox.Text.Remove(pathBox.Text.Length - firstChars.Length - 1);

				if (possibles.Count == 1)
				{
					pathBox.Text = newText + "/" + possibles[0] + "/";
				}

				//if more than one possible, loop through to find the common chars. 
				//e.g my-long-text and my-long-other -> my-long-
				else
				{
					int min = 500;//max length of directory name, increase as required
					string sh = "";
					foreach (string s in possibles)
					{
						if (s.Length < min)
						{
							sh = s;
							min = s.Length;
						}
					}
					char[] newletters = sh.ToCharArray();
					char[] common = new char[newletters.Length];
					bool allok = true;
					for (int i = 0; i < min; i++)
					{
						if (!allok)
							break;
						foreach (string s2 in possibles)
						{
							if (s2 != sh)
							{
								char[] myStr = s2.ToCharArray();
								if (myStr[i] == newletters[i])
									common[i] = myStr[i];
								else
								{
									allok = false;
									break;
								}
							}
						}
					}
					string comm = new string(common);
					comm = comm.Replace("\0", "");
					if (comm == "")
						pathBox.Text = newText + "/" + firstChars;
					else
						pathBox.Text = newText + "/" + comm;
				}
			}
		}
		pathBox.Select(pathBox.Text.Length, 0); //set cursor to end of text
	}
	return false;
}
