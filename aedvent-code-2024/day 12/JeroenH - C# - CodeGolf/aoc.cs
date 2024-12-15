using S=System.Collections.Generic.HashSet<C>;
var p=File.ReadAllLines("input.txt");
List<S> l=[];S v=[];int x,y;
var (w,h)=(p[0].Length,p.Length);
for(y=0;y<h;y++)for(x=0;x<w;x++){var c=new C(x,y);if(!v.Contains(c)){var s=new S();F(c,s);l.Add(s);}}
var p1=l.Select(i=>i.Count*i.Select(c=>4-c.B().Count(i.Contains)).Sum()).Sum();
var p2=l.Select(i=>i.Count*i.Select(c=>{var(d,e,f,g)=((1,1),(0,1),(1,0),0);for(y=0;y<4;y++){var(v,w,x)=(i.Contains(c+d),i.Contains(c+e),i.Contains(c+f));if(!(w||x)||(w&&x&&!v))g++;(d,e,f)=(R(d),R(e),R(f));}return g;(int,int)R((int x,int y)p)=>(p.y,-p.x);}).Sum()).Sum();
Console.WriteLine((p1,p2));
void F(C c,S s){if(!v.Contains(c)){v.Add(c);s.Add(c);foreach(var n in c.B().Where(n=>n.x>=0&&n.y>=0&&n.x<w&&n.y<h&&p[n.y][n.x]==p[c.y][c.x]))F(n,s);}}
record C(int x,int y){public static C operator +(C l,(int x,int y)d)=>new(l.x+d.x,l.y+d.y);public IEnumerable<C> B()=>new[]{(0,-1),(1,0),(0,1),(-1,0)}.Select(d=>this+d);}
