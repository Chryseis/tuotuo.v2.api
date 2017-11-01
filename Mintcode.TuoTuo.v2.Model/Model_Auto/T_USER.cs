using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：T_USER
    public partial class T_USER:ModelBase
    {

                    
         #region  属性描述：ID
         
		 /// <summary>
         /// ID
         /// </summary>
         private int  _ID;
         
		 /// <summary>
         /// ID
         /// </summary>
         [Id(true)]
		 public int ID 
         {
           get
            {
                 return  _ID ;
            }
            set
            {
                Fields.Add("ID");   
                 _ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户名
         
		 /// <summary>
         /// 用户名
         /// </summary>
         private string  _U_USER_NAME;
         
		 /// <summary>
         /// 用户名
         /// </summary>
         [Column]
		 public string U_USER_NAME 
         {
           get
            {
                 return  _U_USER_NAME ;
            }
            set
            {
                Fields.Add("U_USER_NAME");   
                 _U_USER_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户真实姓名
         
		 /// <summary>
         /// 用户真实姓名
         /// </summary>
         private string  _U_USER_TRUE_NAME;
         
		 /// <summary>
         /// 用户真实姓名
         /// </summary>
         [Column]
		 public string U_USER_TRUE_NAME 
         {
           get
            {
                 return  _U_USER_TRUE_NAME ;
            }
            set
            {
                Fields.Add("U_USER_TRUE_NAME");   
                 _U_USER_TRUE_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户级别(0=普通用户 1=待扩展)
         
		 /// <summary>
         /// 用户级别(0=普通用户 1=待扩展)
         /// </summary>
         private int  _U_LEVEL;
         
		 /// <summary>
         /// 用户级别(0=普通用户 1=待扩展)
         /// </summary>
         [Column]
		 public int U_LEVEL 
         {
           get
            {
                 return  _U_LEVEL ;
            }
            set
            {
                Fields.Add("U_LEVEL");   
                 _U_LEVEL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户密码
         
		 /// <summary>
         /// 用户密码
         /// </summary>
         private string  _U_PASSWORD;
         
		 /// <summary>
         /// 用户密码
         /// </summary>
         [Column]
		 public string U_PASSWORD 
         {
           get
            {
                 return  _U_PASSWORD ;
            }
            set
            {
                Fields.Add("U_PASSWORD");   
                 _U_PASSWORD  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户性别 0=男 1=女
         
		 /// <summary>
         /// 用户性别 0=男 1=女
         /// </summary>
         private int?  _U_SEX;
         
		 /// <summary>
         /// 用户性别 0=男 1=女
         /// </summary>
         [Column]
		 public int? U_SEX 
         {
           get
            {
                 return  _U_SEX ;
            }
            set
            {
                Fields.Add("U_SEX");   
                 _U_SEX  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户邮箱
         
		 /// <summary>
         /// 用户邮箱
         /// </summary>
         private string  _U_EMAIL;
         
		 /// <summary>
         /// 用户邮箱
         /// </summary>
         [Column]
		 public string U_EMAIL 
         {
           get
            {
                 return  _U_EMAIL ;
            }
            set
            {
                Fields.Add("U_EMAIL");   
                 _U_EMAIL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：
         
		 /// <summary>
         /// 
         /// </summary>
         private string  _U_MOBILE;
         
		 /// <summary>
         /// 
         /// </summary>
         [Column]
		 public string U_MOBILE 
         {
           get
            {
                 return  _U_MOBILE ;
            }
            set
            {
                Fields.Add("U_MOBILE");   
                 _U_MOBILE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：
         
		 /// <summary>
         /// 
         /// </summary>
         private string  _U_AVATAR;
         
		 /// <summary>
         /// 
         /// </summary>
         [Column]
		 public string U_AVATAR 
         {
           get
            {
                 return  _U_AVATAR ;
            }
            set
            {
                Fields.Add("U_AVATAR");   
                 _U_AVATAR  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户状态
         
		 /// <summary>
         /// 用户状态
         /// </summary>
         private int  _U_STATUS;
         
		 /// <summary>
         /// 用户状态
         /// </summary>
         [Column]
		 public int U_STATUS 
         {
           get
            {
                 return  _U_STATUS ;
            }
            set
            {
                Fields.Add("U_STATUS");   
                 _U_STATUS  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：最后登陆时间
         
		 /// <summary>
         /// 最后登陆时间
         /// </summary>
         private DateTime  _U_LAST_LOGIN_TIME;
         
		 /// <summary>
         /// 最后登陆时间
         /// </summary>
         [Column]
		 public DateTime U_LAST_LOGIN_TIME 
         {
           get
            {
                 return  _U_LAST_LOGIN_TIME ;
            }
            set
            {
                Fields.Add("U_LAST_LOGIN_TIME");   
                 _U_LAST_LOGIN_TIME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：创建时间
         
		 /// <summary>
         /// 创建时间
         /// </summary>
         private DateTime  _CREATE_TIME;
         
		 /// <summary>
         /// 创建时间
         /// </summary>
         [Column]
		 public DateTime CREATE_TIME 
         {
           get
            {
                 return  _CREATE_TIME ;
            }
            set
            {
                Fields.Add("CREATE_TIME");   
                 _CREATE_TIME  = value;
            }
         }     
        
         #endregion          
       
    }
}



	