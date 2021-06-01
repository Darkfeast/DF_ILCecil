using System;
using ILRuntime.Mono.Cecil;
using ILRuntime.Mono.Cecil.Cil;
using System.Reflection;
using System.IO;
//using Mono.Cecil;
//using Mono.Cecil.Cil;
class Program
{
    static void Main(string[] ar)
    {
        string p = Directory.GetCurrentDirectory();
        p=p.Replace("Inject", "");
        
        string path = p+@"\Cecil2.exe";
        string path2 =p+@"\Cecil2Modify.exe";
        if (File.Exists(path))
        {
            DF.Log("exist!!!");
        }
        else
        {
            DF.Log(path);
        }

        //AssemblyDefinition assembly = AssemblyFactory.GetAssembly("Cecil.Program.exe");
        AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path);
        DF.Log($"entryPoint  {assembly.EntryPoint.Name}");

        //TypeDefinition type = assembly.MainModule.Types["Cecil.TestType"];
        TypeDefinition type = assembly.MainModule.GetType("Cecil2.TestType");
        MethodDefinition sayHello = null;
        foreach(MethodDefinition md in type.Methods)
        {
            if(md.Name=="SayHello")
            {
                sayHello = md;
            }
        }
 

        //Console.WriteLine(string value)方法
        MethodInfo writeLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string)});

        //Console.WriteLine方法导入MainModule，并返回在AssemblyDefinition中的引用方式
        MethodReference writeLineRef = assembly.MainModule.ImportReference(writeLine);

        //在SayHello方法开始位置插入一条trace语句
        //  Console.WriteLine(">>Intercepting ");
        //如果插入的语句需要使用函数入参，则必须插入在OpCodes.Ldarg等指令之后

        //CilWorker worker = sayHello.Body.CilWorker;
        ILProcessor ilProcessor= sayHello.Body.GetILProcessor();
        foreach(var v in  sayHello.Body.Instructions)
        {
            DF.Log("IL " + v +"   "+v.OpCode, E_ColorType.Cyan);
        }

        Instruction ldstr = ilProcessor.Create(OpCodes.Ldstr, ">>Intercepting " + sayHello.Name);
        Instruction call = ilProcessor.Create(OpCodes.Call, writeLineRef);
        Instruction first = sayHello.Body.Instructions[0];

        ilProcessor.InsertBefore(first, call);
        ilProcessor.InsertBefore(call, ldstr);
        DF.LogLine(E_ColorType.Cyan);
        foreach(var v in  sayHello.Body.Instructions)
        {
            DF.Log("IL " + v +"   "+v.OpCode, E_ColorType.DarkCyan);
        }

        //在SayHello方法结束位置插入一条trace语句
        //  Console.WriteLine(">>Intercepted ");
        //语句必须插入在OpCodes.Ret指令的前面

        int offset = sayHello.Body.Instructions.Count - 1;
        DF.Log($"instructionsCount {offset + 1}",E_ColorType.Magenta);
        Instruction last = sayHello.Body.Instructions[offset--];
        while(last.OpCode== OpCodes.Nop|| last.OpCode==OpCodes.Ret)
        {
            last = sayHello.Body.Instructions[offset--];
        }

        DF.Log($"last  {last}");

        ldstr = ilProcessor.Create(OpCodes.Ldstr, ">>Intercepted2 " + sayHello.Name);
        ilProcessor.InsertAfter(last, ldstr);
        ilProcessor.InsertAfter(ldstr, call);

        DF.LogLine(  E_ColorType.Yellow);
        foreach(var v in  sayHello.Body.Instructions)
        {
            DF.Log("IL " + v +"   "+v.OpCode, E_ColorType.DarkGreen);
        }

        //把SayHello方法改为虚方法
        sayHello.IsVirtual = true;
        //给SayHello方法添加一个SerializableAttribute
        //特性 f12 定位到的是构造方法
        //CustomAttribute new的传入参数 是一个 constructor
        CustomAttribute attr = new CustomAttribute(
                assembly.MainModule.ImportReference(typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes))
            );
        sayHello.CustomAttributes.Add(attr);
        //assembly.Write();
        //assembly.MainModule = 

        //sayHello.
        DF.LogLine(E_ColorType.Red);
        DF.Log("判断类型是不是 TypeSpecification");
        foreach(var v in assembly.MainModule.GetTypes())
        {
            DF.Log($"name  {v.Name}   fm  {v.FullName}   ref {v.GetElementType()}   ");
            TypeReference vv = v;
            if(vv is TypeSpecification)
            {
                DF.Log($"spe  {((TypeSpecification)vv).ElementType}    ",E_ColorType.Cyan);
                DF.Log($"spe2  {((TypeSpecification)vv).GetElementType()}",E_ColorType.Green);
            }
            else
            {
                DF.Log("################");
            }
         
        }


        DF.Log("判断方法是否为实例方法");
        foreach (var v in type.Methods)
        {
            if (v.HasThis)
            {
                DF.Log($"instance Method   {v.FullName}",E_ColorType.Green);
            }
            else
            { 
                DF.Log($"static  Method   {v.FullName}",E_ColorType.Cyan);
            }
        }


        DF.LogLine();
        DF.Log(assembly.MainModule.ToString());
        DF.Log(assembly.MainModule.HasDebugHeader);
        //assembly.Write();
        assembly.Write(path2);
        DF.Log("Assembly modified successfully!");

        DF.LogLine();


        Console.ReadKey();
    }
}


