using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
namespace Mintcode.TuoTuo.v2.Model

{
    // 类描述：
    public partial class T_RELEASE_SPRINT:ModelBase
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
                  
         #region  属性描述：迭代ID
         
		 /// <summary>
         /// 迭代ID
         /// </summary>
         private int  _R_RELEASE_ID;
         
		 /// <summary>
         /// 迭代ID
         /// </summary>
         [Column]
		 public int R_RELEASE_ID 
         {
           get
            {
                 return  _R_RELEASE_ID ;
            }
            set
            {
                Fields.Add("R_RELEASE_ID");   
                 _R_RELEASE_ID  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：开始时间
         
		 /// <summary>
         /// 开始时间
         /// </summary>
         private DateTime  _R_START_TIME;
         
		 /// <summary>
         /// 开始时间
         /// </summary>
         [Column]
		 public DateTime R_START_TIME 
         {
           get
            {
                 return  _R_START_TIME ;
            }
            set
            {
                Fields.Add("R_START_TIME");   
                 _R_START_TIME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：结束时间
         
		 /// <summary>
         /// 结束时间
         /// </summary>
         private DateTime  _R_END_TIME;
         
		 /// <summary>
         /// 结束时间
         /// </summary>
         [Column]
		 public DateTime R_END_TIME 
         {
           get
            {
                 return  _R_END_TIME ;
            }
            set
            {
                Fields.Add("R_END_TIME");   
                 _R_END_TIME  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：
         
		 /// <summary>
         /// 
         /// </summary>
         private int  _R_NO;
         
		 /// <summary>
         /// 
         /// </summary>
         [Column]
		 public int R_NO 
         {
           get
            {
                 return  _R_NO ;
            }
            set
            {
                Fields.Add("R_NO");   
                 _R_NO  = value;
            }
         }     
        
         #endregion          
                  
         #region  属性描述：0 默认状态 1 当前正在使用
         
		 /// <summary>
         /// 0 默认状态 1 当前正在使用
         /// </summary>
         private int  _R_STATE;
         
		 /// <summary>
         /// 0 默认状态 1 当前正在使用
         /// </summary>
         [Column]
		 public int R_STATE 
         {
           get
            {
                 return  _R_STATE ;
            }
            set
            {
                Fields.Add("R_STATE");   
                 _R_STATE  = value;
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



	