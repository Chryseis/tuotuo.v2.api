using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：TimeSheet总表
    public partial class T_TIME_SHEET:ModelBase
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
                  
         #region  属性描述：用户ID
         
		 /// <summary>
         /// 用户ID
         /// </summary>
         private int  _TS_USER_ID;
         
		 /// <summary>
         /// 用户ID
         /// </summary>
         [Column]
		 public int TS_USER_ID 
         {
           get
            {
                 return  _TS_USER_ID ;
            }
            set
            {
                Fields.Add("TS_USER_ID");   
                 _TS_USER_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户邮箱
         
		 /// <summary>
         /// 用户邮箱
         /// </summary>
         private string  _TS_USER_MAIL;
         
		 /// <summary>
         /// 用户邮箱
         /// </summary>
         [Column]
		 public string TS_USER_MAIL 
         {
           get
            {
                 return  _TS_USER_MAIL ;
            }
            set
            {
                Fields.Add("TS_USER_MAIL");   
                 _TS_USER_MAIL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：用户姓名
         
		 /// <summary>
         /// 用户姓名
         /// </summary>
         private string  _TS_USER_NAME;
         
		 /// <summary>
         /// 用户姓名
         /// </summary>
         [Column]
		 public string TS_USER_NAME 
         {
           get
            {
                 return  _TS_USER_NAME ;
            }
            set
            {
                Fields.Add("TS_USER_NAME");   
                 _TS_USER_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：TimeSheet日期
         
		 /// <summary>
         /// TimeSheet日期
         /// </summary>
         private DateTime  _TS_DATE;
         
		 /// <summary>
         /// TimeSheet日期
         /// </summary>
         [Column]
		 public DateTime TS_DATE 
         {
           get
            {
                 return  _TS_DATE ;
            }
            set
            {
                Fields.Add("TS_DATE");   
                 _TS_DATE  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：TimeSheet日期时间戳
         
		 /// <summary>
         /// TimeSheet日期时间戳
         /// </summary>
         private long  _TS_TIMESTAMP;
         
		 /// <summary>
         /// TimeSheet日期时间戳
         /// </summary>
         [Column]
		 public long TS_TIMESTAMP 
         {
           get
            {
                 return  _TS_TIMESTAMP ;
            }
            set
            {
                Fields.Add("TS_TIMESTAMP");   
                 _TS_TIMESTAMP  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：TimeSheet状态，0=待填写，1=已提交，2=已审核
         
		 /// <summary>
         /// TimeSheet状态，0=待填写，1=已提交，2=已审核
         /// </summary>
         private int  _TS_STATUS;
         
		 /// <summary>
         /// TimeSheet状态，0=待填写，1=已提交，2=已审核
         /// </summary>
         [Column]
		 public int TS_STATUS 
         {
           get
            {
                 return  _TS_STATUS ;
            }
            set
            {
                Fields.Add("TS_STATUS");   
                 _TS_STATUS  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：TimeSheet提交时间
         
		 /// <summary>
         /// TimeSheet提交时间
         /// </summary>
         private DateTime?  _TS_SUBMIT_TIME;
         
		 /// <summary>
         /// TimeSheet提交时间
         /// </summary>
         [Column]
		 public DateTime? TS_SUBMIT_TIME 
         {
           get
            {
                 return  _TS_SUBMIT_TIME ;
            }
            set
            {
                Fields.Add("TS_SUBMIT_TIME");   
                 _TS_SUBMIT_TIME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：审核用户ID
         
		 /// <summary>
         /// 审核用户ID
         /// </summary>
         private int?  _TS_APPROVAL_USER_ID;
         
		 /// <summary>
         /// 审核用户ID
         /// </summary>
         [Column]
		 public int? TS_APPROVAL_USER_ID 
         {
           get
            {
                 return  _TS_APPROVAL_USER_ID ;
            }
            set
            {
                Fields.Add("TS_APPROVAL_USER_ID");   
                 _TS_APPROVAL_USER_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：审核用户邮箱
         
		 /// <summary>
         /// 审核用户邮箱
         /// </summary>
         private string  _TS_APPROVAL_USER_MAIL;
         
		 /// <summary>
         /// 审核用户邮箱
         /// </summary>
         [Column]
		 public string TS_APPROVAL_USER_MAIL 
         {
           get
            {
                 return  _TS_APPROVAL_USER_MAIL ;
            }
            set
            {
                Fields.Add("TS_APPROVAL_USER_MAIL");   
                 _TS_APPROVAL_USER_MAIL  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：审核用户姓名
         
		 /// <summary>
         /// 审核用户姓名
         /// </summary>
         private string  _TS_APPROVAL_USER_NAME;
         
		 /// <summary>
         /// 审核用户姓名
         /// </summary>
         [Column]
		 public string TS_APPROVAL_USER_NAME 
         {
           get
            {
                 return  _TS_APPROVAL_USER_NAME ;
            }
            set
            {
                Fields.Add("TS_APPROVAL_USER_NAME");   
                 _TS_APPROVAL_USER_NAME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：审核时间
         
		 /// <summary>
         /// 审核时间
         /// </summary>
         private DateTime?  _TS_APPROVAL_TIME;
         
		 /// <summary>
         /// 审核时间
         /// </summary>
         [Column]
		 public DateTime? TS_APPROVAL_TIME 
         {
           get
            {
                 return  _TS_APPROVAL_TIME ;
            }
            set
            {
                Fields.Add("TS_APPROVAL_TIME");   
                 _TS_APPROVAL_TIME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：审核结果，0=疑问，1=合格，2=优秀
         
		 /// <summary>
         /// 审核结果，0=疑问，1=合格，2=优秀
         /// </summary>
         private int?  _TS_APPROVAL_RESULT;
         
		 /// <summary>
         /// 审核结果，0=疑问，1=合格，2=优秀
         /// </summary>
         [Column]
		 public int? TS_APPROVAL_RESULT 
         {
           get
            {
                 return  _TS_APPROVAL_RESULT ;
            }
            set
            {
                Fields.Add("TS_APPROVAL_RESULT");   
                 _TS_APPROVAL_RESULT  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：评论
         
		 /// <summary>
         /// 评论
         /// </summary>
         private string  _TS_APPROVAL_COMMENT;
         
		 /// <summary>
         /// 评论
         /// </summary>
         [Column]
		 public string TS_APPROVAL_COMMENT 
         {
           get
            {
                 return  _TS_APPROVAL_COMMENT ;
            }
            set
            {
                Fields.Add("TS_APPROVAL_COMMENT");   
                 _TS_APPROVAL_COMMENT  = value;
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



	