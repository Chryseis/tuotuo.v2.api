using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：
    public partial class T_TASK:ModelBase
    {

                    
         #region  属性描述：
         
		 /// <summary>
         /// 
         /// </summary>
         private int  _ID;
         
		 /// <summary>
         /// 
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
                  
         #region  属性描述：产品 ID
         
		 /// <summary>
         /// 产品 ID
         /// </summary>
         private int?  _T_BACKLOG_ID;
         
		 /// <summary>
         /// 产品 ID
         /// </summary>
         [Column]
		 public int? T_BACKLOG_ID 
         {
           get
            {
                 return  _T_BACKLOG_ID ;
            }
            set
            {
                Fields.Add("T_BACKLOG_ID");   
                 _T_BACKLOG_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目ID
         
		 /// <summary>
         /// 项目ID
         /// </summary>
         private int  _P_PROJECT_ID;
         
		 /// <summary>
         /// 项目ID
         /// </summary>
         [Column]
		 public int P_PROJECT_ID 
         {
           get
            {
                 return  _P_PROJECT_ID ;
            }
            set
            {
                Fields.Add("P_PROJECT_ID");   
                 _P_PROJECT_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：项目名称
         
		 /// <summary>
         /// 项目名称
         /// </summary>
         private string  _P_PROJECT_NAME;
         
		 /// <summary>
         /// 项目名称
         /// </summary>
         [Column]
		 public string P_PROJECT_NAME 
         {
           get
            {
                 return  _P_PROJECT_NAME ;
            }
            set
            {
                Fields.Add("P_PROJECT_NAME");   
                 _P_PROJECT_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：任务标题
         
		 /// <summary>
         /// 任务标题
         /// </summary>
         private string  _T_TITLE;
         
		 /// <summary>
         /// 任务标题
         /// </summary>
         [Column]
		 public string T_TITLE 
         {
           get
            {
                 return  _T_TITLE ;
            }
            set
            {
                Fields.Add("T_TITLE");   
                 _T_TITLE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：任务内容
         
		 /// <summary>
         /// 任务内容
         /// </summary>
         private string  _T_CONENT;
         
		 /// <summary>
         /// 任务内容
         /// </summary>
         [Column]
		 public string T_CONENT 
         {
           get
            {
                 return  _T_CONENT ;
            }
            set
            {
                Fields.Add("T_CONENT");   
                 _T_CONENT  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：指定人
         
		 /// <summary>
         /// 指定人
         /// </summary>
         private string  _T_ASSIGNED_NAME;
         
		 /// <summary>
         /// 指定人
         /// </summary>
         [Column]
		 public string T_ASSIGNED_NAME 
         {
           get
            {
                 return  _T_ASSIGNED_NAME ;
            }
            set
            {
                Fields.Add("T_ASSIGNED_NAME");   
                 _T_ASSIGNED_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：指定人邮箱
         
		 /// <summary>
         /// 指定人邮箱
         /// </summary>
         private string  _T_ASSIGNED_EMAIL;
         
		 /// <summary>
         /// 指定人邮箱
         /// </summary>
         [Column]
		 public string T_ASSIGNED_EMAIL 
         {
           get
            {
                 return  _T_ASSIGNED_EMAIL ;
            }
            set
            {
                Fields.Add("T_ASSIGNED_EMAIL");   
                 _T_ASSIGNED_EMAIL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：1 计划 2 进行 3 完成4 删除
         
		 /// <summary>
         /// 1 计划 2 进行 3 完成4 删除
         /// </summary>
         private int  _T_STATE;
         
		 /// <summary>
         /// 1 计划 2 进行 3 完成4 删除
         /// </summary>
         [Column]
		 public int T_STATE 
         {
           get
            {
                 return  _T_STATE ;
            }
            set
            {
                Fields.Add("T_STATE");   
                 _T_STATE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：分类
         
		 /// <summary>
         /// 分类
         /// </summary>
         private string  _T_TYPE_NAME;
         
		 /// <summary>
         /// 分类
         /// </summary>
         [Column]
		 public string T_TYPE_NAME 
         {
           get
            {
                 return  _T_TYPE_NAME ;
            }
            set
            {
                Fields.Add("T_TYPE_NAME");   
                 _T_TYPE_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：预估时间
         
		 /// <summary>
         /// 预估时间
         /// </summary>
         private decimal  _T_TIME;
         
		 /// <summary>
         /// 预估时间
         /// </summary>
         [Column]
		 public decimal T_TIME 
         {
           get
            {
                 return  _T_TIME ;
            }
            set
            {
                Fields.Add("T_TIME");   
                 _T_TIME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：创建人
         
		 /// <summary>
         /// 创建人
         /// </summary>
         private string  _CREATE_USER_MAIL;
         
		 /// <summary>
         /// 创建人
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



	