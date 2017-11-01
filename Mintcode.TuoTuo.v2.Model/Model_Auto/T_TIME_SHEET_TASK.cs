using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：TimeSheet 详情表
    public partial class T_TIME_SHEET_TASK:ModelBase
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
                  
         #region  属性描述：Sheet ID
         
		 /// <summary>
         /// Sheet ID
         /// </summary>
         private int  _TS_ID;
         
		 /// <summary>
         /// Sheet ID
         /// </summary>
         [Column]
		 public int TS_ID 
         {
           get
            {
                 return  _TS_ID ;
            }
            set
            {
                Fields.Add("TS_ID");   
                 _TS_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目ID
         
		 /// <summary>
         /// 项目ID
         /// </summary>
         private int  _P_ID;
         
		 /// <summary>
         /// 项目ID
         /// </summary>
         [Column]
		 public int P_ID 
         {
           get
            {
                 return  _P_ID ;
            }
            set
            {
                Fields.Add("P_ID");   
                 _P_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目名称
         
		 /// <summary>
         /// 项目名称
         /// </summary>
         private string  _P_NAME;
         
		 /// <summary>
         /// 项目名称
         /// </summary>
         [Column]
		 public string P_NAME 
         {
           get
            {
                 return  _P_NAME ;
            }
            set
            {
                Fields.Add("P_NAME");   
                 _P_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：工作详情
         
		 /// <summary>
         /// 工作详情
         /// </summary>
         private string  _TST_DETAIL;
         
		 /// <summary>
         /// 工作详情
         /// </summary>
         [Column]
		 public string TST_DETAIL 
         {
           get
            {
                 return  _TST_DETAIL ;
            }
            set
            {
                Fields.Add("TST_DETAIL");   
                 _TST_DETAIL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：工作时长
         
		 /// <summary>
         /// 工作时长
         /// </summary>
         private decimal  _TST_TIME;
         
		 /// <summary>
         /// 工作时长
         /// </summary>
         [Column]
		 public decimal TST_TIME 
         {
           get
            {
                 return  _TST_TIME ;
            }
            set
            {
                Fields.Add("TST_TIME");   
                 _TST_TIME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户ID
         
		 /// <summary>
         /// 用户ID
         /// </summary>
         private int  _TST_USER_ID;
         
		 /// <summary>
         /// 用户ID
         /// </summary>
         [Column]
		 public int TST_USER_ID 
         {
           get
            {
                 return  _TST_USER_ID ;
            }
            set
            {
                Fields.Add("TST_USER_ID");   
                 _TST_USER_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户邮箱
         
		 /// <summary>
         /// 用户邮箱
         /// </summary>
         private string  _TST_USER_MAIL;
         
		 /// <summary>
         /// 用户邮箱
         /// </summary>
         [Column]
		 public string TST_USER_MAIL 
         {
           get
            {
                 return  _TST_USER_MAIL ;
            }
            set
            {
                Fields.Add("TST_USER_MAIL");   
                 _TST_USER_MAIL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户姓名
         
		 /// <summary>
         /// 用户姓名
         /// </summary>
         private string  _TST_USER_NAME;
         
		 /// <summary>
         /// 用户姓名
         /// </summary>
         [Column]
		 public string TST_USER_NAME 
         {
           get
            {
                 return  _TST_USER_NAME ;
            }
            set
            {
                Fields.Add("TST_USER_NAME");   
                 _TST_USER_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：创建人邮箱
         
		 /// <summary>
         /// 创建人邮箱
         /// </summary>
         private string  _CREATE_USER_MAIL;
         
		 /// <summary>
         /// 创建人邮箱
         /// </summary>
         [Column]
		 public string CREATE_USER_MAIL 
         {
           get
            {
                 return  _CREATE_USER_MAIL ;
            }
            set
            {
                Fields.Add("CREATE_USER_MAIL");   
                 _CREATE_USER_MAIL  = value;
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



	