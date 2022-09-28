using System;
using System.Collections.Generic;
using System.Text;
using EvilMask.Emuera;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.Sub;
using trerror = EvilMask.Emuera.Lang.Error;
namespace MinorShift.Emuera.GameData.Function
{
	internal abstract class FunctionMethod
	{
		public Type ReturnType { get; protected set; }
		protected Type[] argumentTypeArray;
		protected string Name { get; private set; }
		#region EM_私家版_Emuera多言語化改造
		protected enum ArgRefType
		{
			None = 0,
			Var,
			Array1D,
			Array2D,
			Array3D,
			CharacterData,
		}
		protected enum ArgType
		{
			Any = 0, Int, String, RefAny, RefInt, RefString,
			RefAny1D, RefInt1D, RefString1D,
			RefAny2D, RefInt2D, RefString2D, RefAny3D, RefInt3D, RefString3D,
			CharacterData,
		}
		protected sealed class _ArgType
		{
			public _ArgType(Type t, ArgRefType r)
			{
				Type = t; Ref = r;
			}
			public Type Type { get; set; }
			public ArgRefType Ref { get; set; } = ArgRefType.None;

			public static implicit operator _ArgType(ArgType value)
			{
				Type t = null;
				if (value != ArgType.CharacterData)
				{
					switch ((int)value % 3)
					{
						case 0: t = typeof(void); break;
						case 1: t = typeof(Int64); break;
						case 2: t = typeof(string); break;
					}
				}
				ArgRefType r = ArgRefType.None;
				if (value == ArgType.CharacterData) r = ArgRefType.CharacterData;
				else if(value > ArgType.RefString2D) r = ArgRefType.Array3D;
				else if (value > ArgType.RefString1D) r = ArgRefType.Array2D;
				else if (value > ArgType.RefString) r = ArgRefType.Array1D;
				else if (value > ArgType.String) r = ArgRefType.Var;
				return new _ArgType(t, r);
			}
		}
		protected sealed class ArgTypeList
		{
			public List<_ArgType> ArgTypes { get; set; } = new List<_ArgType>();
			public int OmitStart { get; set; } = -1;
		}
		protected ArgTypeList[] argumentTypeArrayEx = null;

		//引数の数・型が一致するかどうかのテスト
		//正しくない場合はエラーメッセージを返す。
		//引数の数が不定である場合や引数の省略を許す場合にはoverrideすること。
		private string CheckArgumentTypeEx(string name, IOperandTerm[] arguments)
		{
			string[] errMsg = new string[argumentTypeArrayEx.Length];
			for (int idx = 0; idx < argumentTypeArrayEx.Length; idx++)
			{
				var list = argumentTypeArrayEx[idx];
				if ((list.OmitStart > -1 && arguments.Length >= list.OmitStart && arguments.Length <= list.ArgTypes.Count)
					|| (list.OmitStart < 0 && arguments.Length == list.ArgTypes.Count))
				{
					// 引数の数が有効
					for (int i = 0; i < Math.Min(arguments.Length, list.ArgTypes.Count); i++)
					{
						if (arguments[i] == null)
						{
							errMsg[idx] = string.Format(Lang.Error.ArgCanNotBeNull.Text, name, i + 1);
							break;
						}
						var rule = list.ArgTypes[i];
						if (rule.Ref != ArgRefType.None && rule.Ref != ArgRefType.CharacterData)
						{
							// 引数の型が違う
							bool error = false;
							bool typeNotMatch = rule.Type != typeof(void) && rule.Type != arguments[i].GetOperandType();
							string errText = null;
							switch (rule.Ref)
							{
								case ArgRefType.Var:
									{ 
										// 普通の場合
										var err = list.ArgTypes[i].Type == typeof(string) ? Lang.Error.ArgIsNotStrVar
											: (list.ArgTypes[i].Type == typeof(Int64) ? Lang.Error.ArgIsNotIntVar : Lang.Error.ArgIsNotVar);
										errText = string.Format(err.Text, name, i + 1);
										break;
									}
								case ArgRefType.Array1D:
									{
										// 一次元配列の場合
										var err = list.ArgTypes[i].Type == typeof(string) ? Lang.Error.ArgIsNotNDStrArray
											: (list.ArgTypes[i].Type == typeof(Int64) ? Lang.Error.ArgIsNotNDIntArray : Lang.Error.ArgIsNotNDArray);
										errText = string.Format(err.Text, name, i + 1, 1);
										break;
									}
								case ArgRefType.Array2D:
									{
										// 二次元配列の場合
										var err = list.ArgTypes[i].Type == typeof(string) ? Lang.Error.ArgIsNotNDStrArray
											: (list.ArgTypes[i].Type == typeof(Int64) ? Lang.Error.ArgIsNotNDIntArray : Lang.Error.ArgIsNotNDArray);
										errText = string.Format(err.Text, name, i + 1, 2);
										break;
									}
								case ArgRefType.Array3D:
									{
										// 三次元配列の場合
										var err = list.ArgTypes[i].Type == typeof(string) ? Lang.Error.ArgIsNotNDStrArray
											: (list.ArgTypes[i].Type == typeof(Int64) ? Lang.Error.ArgIsNotNDIntArray : Lang.Error.ArgIsNotNDArray);
										errText = string.Format(err.Text, name, i + 1, 3);
										break;
									}
							}
							// 引数が引用系
							if ((arguments[i] is VariableTerm varTerm && !(varTerm.Identifier.IsCalc || varTerm.Identifier.IsConst)))
							{
								// 変数の場合
								switch (rule.Ref)
								{
									case ArgRefType.Var: error = !varTerm.Identifier.IsArray1D || typeNotMatch; break;
									case ArgRefType.Array1D: error = !varTerm.Identifier.IsArray1D || typeNotMatch; break;
									case ArgRefType.Array2D: error = !varTerm.Identifier.IsArray1D || typeNotMatch; break;
									case ArgRefType.Array3D: error = !varTerm.Identifier.IsArray1D || typeNotMatch; break;
								}
							}
							else error = true; // 変数ではない
							if (error)
							{
								errMsg[idx] = errText;
								break;
							}
						}
						else if (rule.Ref == ArgRefType.CharacterData && (!(arguments[i] is VariableTerm cvarTerm) || !cvarTerm.Identifier.IsCharacterData))
						{
							// キャラ変数ではない
							errMsg[idx] = string.Format(Lang.Error.ArgIsNotCharacterVar.Text, name, i + 1);
							break;
						}
						else if (rule.Ref == ArgRefType.None && rule.Type != typeof(void) && rule.Type != arguments[i].GetOperandType())
						{
							// 引数の型が違う
							errMsg[idx] = rule.Type == typeof(string) ? string.Format(Lang.Error.ArgIsNotStr.Text, name, i + 1)
								: string.Format(Lang.Error.ArgIsNotInt.Text, name, i + 1);
							break;
						}
					}
					if (errMsg[idx] == null)
						return null;
				}
				else if (arguments.Length > list.ArgTypes.Count)
				{
					// 引数が多すぎる
					errMsg[idx] = string.Format(Lang.Error.TooManyFuncArgs.Text, name);
					continue;
				}
				else
				{
					// 引数が足りない
					errMsg[idx] = string.Format(Lang.Error.NotEnoughArgs.Text, name, list.OmitStart < 0 ? list.ArgTypes.Count : list.OmitStart);
					continue;
				}
			}
			if (argumentTypeArrayEx.Length == 1) return errMsg[0];

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < errMsg.Length; i++)
			{
				sb.Append(string.Format(Lang.Error.NotValidArgsReason.Text, i + 1, errMsg[i]));
				if (i + 1 < errMsg.Length) sb.Append(" | ");
			}
			return string.Format(Lang.Error.NotValidArgs.Text, name, sb.ToString());
		}
		public virtual string CheckArgumentType(string name, IOperandTerm[] arguments)
		{
			if (argumentTypeArrayEx != null)
			{
				return CheckArgumentTypeEx(name, arguments);
			}
			else if (argumentTypeArray!=null)
			{
				if (arguments.Length != argumentTypeArray.Length)
					// return string.Format(Properties.Resources.SyntaxErrMesMethodDefaultArgumentNum0, name);
					return string.Format(Lang.Error.ErrArgsCount.Text, name);
				for (int i = 0; i < argumentTypeArray.Length; i++)
				{
					if (arguments[i] == null)
						// return string.Format(Properties.Resources.SyntaxErrMesMethodDefaultArgumentNotNullable0, name, i + 1);
						return string.Format(Lang.Error.ArgCanNotBeNull.Text, name, i + 1);
					if (argumentTypeArray[i] != arguments[i].GetOperandType())
						// return string.Format(Properties.Resources.SyntaxErrMesMethodDefaultArgumentType0, name, i + 1);
						return argumentTypeArray[i] == typeof(string) ? string.Format(Lang.Error.ArgIsNotStr.Text, name, i + 1)
								: string.Format(Lang.Error.ArgIsNotInt.Text, name, i + 1);
				}
			}
			return null;
		}
		#endregion

		//Argumentが全て定数の時にMethodを解体してよいかどうか。RANDやCharaを参照するものなどは不可
		public bool CanRestructure { get; protected set; }

		//FunctionMethodが固有のRestructure()を持つかどうか
		public bool HasUniqueRestructure { get; protected set; }

		//実際の計算。
		public virtual Int64 GetIntValue(ExpressionMediator exm, IOperandTerm[] arguments) { throw new ExeEE(trerror.ReturnTypeDifferentOrNotImpelemnt.Text); }
		public virtual string GetStrValue(ExpressionMediator exm, IOperandTerm[] arguments) { throw new ExeEE(trerror.ReturnTypeDifferentOrNotImpelemnt.Text); }
		public virtual SingleTerm GetReturnValue(ExpressionMediator exm, IOperandTerm[] arguments)
		{
			if (ReturnType == typeof(Int64))
				return new SingleTerm(GetIntValue(exm, arguments));
			else
				return new SingleTerm(GetStrValue(exm, arguments));
		}

		/// <summary>
		/// 戻り値は全体をRestructureできるかどうか
		/// </summary>
		/// <param name="exm"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public virtual bool UniqueRestructure(ExpressionMediator exm, IOperandTerm[] arguments)
		{ throw new ExeEE(trerror.NotImplement.Text); }


		internal void SetMethodName(string name)
		{
			Name = name;
		}
	}
}
