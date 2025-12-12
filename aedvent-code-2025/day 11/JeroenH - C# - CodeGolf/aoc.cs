var g=File.ReadLines("i").ToDictionary(l=>l[..3],l=>l[5..].Split());Dictionary<(string,int),long>c=[];
long C(string n, int s){s=(s&4)<1?0:s|(n=="dac"?1:n=="fft"?2:0);return n=="out"?s<1|s>6?1:0:c.ContainsKey((n,s))?c[(n,s)]:c[(n,s)]=g[n].Sum(a=>C(a,s));}
Console.Write(C("you",0)+";"+C("svr",4));