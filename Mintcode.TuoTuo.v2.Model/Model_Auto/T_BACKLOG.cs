using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：
    public partial class T_BACKLOG:ModelBase
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
                  
         #region  属性描述：团队ID
         
		 /// <summary>
         /// 团队ID
         /// </summary>
         private int  _T_TEAM_ID;
         
		 /// <summary>
         /// 团队ID
         /// </summary>
         [Column]
		 public int T_TEAM_ID 
         {
           get
            {
                 return  _T_TEAM_ID ;
            }
            set
            {
                Fields.Add("T_TEAM_ID");   
                 _T_TEAM_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：没有填0
         
		 /// <summary>
         /// 没有填0
         /// </summary>
         private int  _P_PROJECT_ID;
         
		 /// <summary>
         /// 没有填0
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
                  
         #region  属性描述：没有填0
         
		 /// <summary>
         /// 没有填0
         /// </summary>
         private int  _R_SPRINT_ID;
         
		 /// <summary>
         /// 没有填0
         /// </summary>
         [Column]
		 public int R_SPRINT_ID 
         {
           get
            {
                 return  _R_SPRINT_ID ;
            }
            set
            {
                Fields.Add("R_SPRINT_ID");   
                 _R_SPRINT_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：标题
         
		 /// <summary>
         /// 标题
         /// </summary>
         private string  _B_TITLE;
         
		 /// <summary>
         /// 标题
         /// </summary>
         [Column]
		 public string B_TITLE 
         {
           get
            {
                 return  _B_TITLE ;
            }
            set
            {
                Fields.Add("B_TITLE");   
                 _B_TITLE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：内容
         
		 /// <summary>
         /// 内容
         /// </summary>
         private string  _B_CONTENT;
         
		 /// <summary>
         /// 内容
         /// </summary>
         [Column]
		 public string B_CONTENT 
         {
           get
            {
                 return  _B_CONTENT ;
            }
            set
            {
                Fields.Add("B_CONTENT");   
                 _B_CONTENT  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：评审标准
         
		 /// <summary>
         /// 评审标准
         /// </summary>
         private string  _B_STANDARD;
         
		 /// <summary>
         /// 评审标准
         /// </summary>
         [Column]
		 public string B_STANDARD 
         {
           get
            {
                 return  _B_STANDARD ;
            }
            set
            {
                Fields.Add("B_STANDARD");   
                 _B_STANDARD  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：1 new 2 inprogerss 3 done 4 remove 5 fail
         
		 /// <summary>
         /// 1 new 2 inprogerss 3 done 4 remove 5 fail
         /// </summary>
         private int  _B_STATE;
         
		 /// <summary>
         /// 1 new 2 inprogerss 3 done 4 remove 5 fail
         /// </summary>
         [Column]
		 public int B_STATE 
         {
           get
            {
                 return  _B_STATE ;
            }
            set
            {
                Fields.Add("B_STATE");   
                 _B_STATE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：优先级
         
		 /// <summary>
         /// 优先级
         /// </summary>
         private int?  _B_LEVEL;
         
		 /// <summary>
         /// 优先级
         /// </summary>
         [Column]
		 public int? B_LEVEL 
         {
           get
            {
                 return  _B_LEVEL ;
            }
            set
            {
                Fields.Add("B_LEVEL");   
                 _B_LEVEL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：负责人
         
		 /// <summary>
         /// 负责人
         /// </summary>
         private string  _B_ASSIGNED_NAME;
         
		 /// <summary>
         /// 负责人
         /// </summary>
         [Column]
		 public string B_ASSIGNED_NAME 
         {
           get
            {
                 return  _B_ASSIGNED_NAME ;
            }
            set
            {
                Fields.Add("B_ASSIGNED_NAME");   
                 _B_ASSIGNED_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：负责人邮箱
         
		 /// <summary>
         /// 负责人邮箱
         /// </summary>
         private string  _B_ASSIGNED_EMAIL;
         
		 /// <summary>
         /// 负责人邮箱
         /// </summary>
         [Column]
		 public string B_ASSIGNED_EMAIL 
         {
           get
            {
                 return  _B_ASSIGNED_EMAIL ;
            }
            set
            {
                Fields.Add("B_ASSIGNED_EMAIL");   
                 _B_ASSIGNED_EMAIL  = value;
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



	