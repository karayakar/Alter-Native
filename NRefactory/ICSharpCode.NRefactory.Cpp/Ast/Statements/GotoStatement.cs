﻿// 
// GotoStatement.cs
//
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using ICSharpCode.NRefactory.Cpp.Ast;
namespace ICSharpCode.NRefactory.Cpp
{
	/// <summary>
	/// "goto Label;"
	/// </summary>
	public class GotoStatement : Statement
	{
		public GotoStatement ()
		{
		}
		
		public GotoStatement (string label)
		{
			this.Label = label;
		}
		
		public CppTokenNode GotoToken {
			get { return GetChildByRole (Roles.Keyword); }
		}
		
		public string Label {
			get {
				return GetChildByRole (Roles.Identifier).Name;
			}
			set {
				if (string.IsNullOrEmpty(value))
					SetChildByRole(Roles.Identifier, null);
				else
					SetChildByRole(Roles.Identifier, Identifier.Create (value, TextLocation.Empty));
			}
		}
		
		public CppTokenNode SemicolonToken {
			get { return GetChildByRole (Roles.Semicolon); }
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data = default(T))
		{
			return visitor.VisitGotoStatement (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			GotoStatement o = other as GotoStatement;
			return o != null && MatchString(this.Label, o.Label);
		}
	}
	
	/// <summary>
	/// or "goto case LabelExpression;"
	/// </summary>
	public class GotoCaseStatement : Statement
	{
		public static readonly Role<CppTokenNode> CaseKeywordRole = new Role<CppTokenNode>("CaseKeyword", CppTokenNode.Null);
		
		public CppTokenNode GotoToken {
			get { return GetChildByRole (Roles.Keyword); }
		}
		
		public CppTokenNode CaseToken {
			get { return GetChildByRole (CaseKeywordRole); }
		}
		
		/// <summary>
		/// Used for "goto case LabelExpression;"
		/// </summary>
		public Expression LabelExpression {
			get { return GetChildByRole (Roles.Expression); }
			set { SetChildByRole (Roles.Expression, value); }
		}
		
		public CppTokenNode SemicolonToken {
			get { return GetChildByRole (Roles.Semicolon); }
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data = default(T))
		{
			return visitor.VisitGotoCaseStatement (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			GotoCaseStatement o = other as GotoCaseStatement;
			return o != null && this.LabelExpression.DoMatch(o.LabelExpression, match);
		}
	}
	
	/// <summary>
	/// or "goto default;"
	/// </summary>
	public class GotoDefaultStatement : Statement
	{
		public static readonly Role<CppTokenNode> DefaultKeywordRole = new Role<CppTokenNode>("DefaultKeyword", CppTokenNode.Null);
		
		public CppTokenNode GotoToken {
			get { return GetChildByRole (Roles.Keyword); }
		}
		
		public CppTokenNode DefaultToken {
			get { return GetChildByRole (DefaultKeywordRole); }
		}
		
		public CppTokenNode SemicolonToken {
			get { return GetChildByRole (Roles.Semicolon); }
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data = default(T))
		{
			return visitor.VisitGotoDefaultStatement (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			GotoDefaultStatement o = other as GotoDefaultStatement;
			return o != null;
		}
	}
}