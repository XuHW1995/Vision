#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class ReadAndSaveVideoInfoWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ReadAndSaveVideoInfo);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadVideoTime", _m_ReadVideoTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SaveVideoTime", _m_SaveVideoTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetShowImageSpriteByUrl", _m_GetShowImageSpriteByUrl_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetImageSavePathByUrl", _m_GetImageSavePathByUrl_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					ReadAndSaveVideoInfo __cl_gen_ret = new ReadAndSaveVideoInfo();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ReadAndSaveVideoInfo constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadVideoTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string videourl = LuaAPI.lua_tostring(L, 1);
                    
                        int __cl_gen_ret = ReadAndSaveVideoInfo.ReadVideoTime( videourl );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveVideoTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string videourl = LuaAPI.lua_tostring(L, 1);
                    int videotime = LuaAPI.xlua_tointeger(L, 2);
                    
                    ReadAndSaveVideoInfo.SaveVideoTime( videourl, videotime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetShowImageSpriteByUrl_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string videourl = LuaAPI.lua_tostring(L, 1);
                    
                        UnityEngine.Sprite __cl_gen_ret = ReadAndSaveVideoInfo.GetShowImageSpriteByUrl( videourl );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetImageSavePathByUrl_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string videourl = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = ReadAndSaveVideoInfo.GetImageSavePathByUrl( videourl );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
