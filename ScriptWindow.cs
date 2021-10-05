// using System;
// using System.Dynamic;
// using FairyGUI;
//
// namespace SkySwordKill.Next
// {
//     public class ScriptWindow : Window
//     {
//         public ExpressionEvaluator evaluator;
//         public string bindScript;
//
//         protected override void OnInit()
//         {
//             evaluator.Context = this;
//             evaluator.ScriptEvaluate(bindScript);
//         }
//
//         protected override void OnShown()
//         {
//             TryInvoke("onShown");   
//         }
//         
//         protected override void OnHide()
//         {
//             TryInvoke("onHide");   
//         }
//
//         protected void TryInvoke(string method)
//         {
//             evaluator.ScriptEvaluate($@"
// if ({method} != null)
// {{
//     {method}.Invoke();
// }}
// ");
//         }
//         
//         public Action GetAction(string methodName)
//         {
//             return () =>
//             {
//                 TryInvoke(methodName);
//             };
//         }
//     }
// }