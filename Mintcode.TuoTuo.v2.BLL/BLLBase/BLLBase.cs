using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;

namespace Mintcode.TuoTuo.v2.BLL
{
    public class BLLBase<T> : IDisposable
   where T : class,new()
    {

        public BLLBase(string _connectionName)
        {
            this.connectionName = _connectionName;
        }
        /// <summary>
        /// connectionStrings的连接字符串名称
        /// </summary>
        public string connectionName = "strAppmySql";

        public string paramHostName;
        /// <summary>
        ///   数据库基础类
        /// </summary>
        protected DbBase DbContext
        {
            get
            {
                return GetDbContext();
            }
        }
        /// <summary>
        /// 数据库私有
        /// </summary>
        private DbBase _dbcontext;

        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <returns></returns>
        protected DbBase GetDbContext()
        {
            if (_dbcontext != null)
            {
                return _dbcontext;
            }
            _dbcontext = new DbBase(connectionName);

            return _dbcontext;
        }

        #region 增加、修改、删除

        /// <summary>
        /// 新增单个实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual bool Add(T entity)
        {
           return DbContext.Insert(entity);
        }

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entityList"></param>
        public virtual void BatchAdd(List<T> entityList)
        {
            //自动新增事务处理
            using (var tran = DbContext.DbTransaction)
            {
                DbContext.InsertBatch(entityList);
                tran.Commit();
            }
        }

        /// <summary>
        ///  更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Update(T entity)
        {
            return DbContext.Update(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        public virtual void Delete(DapperExQuery<T> query)
        {
            DbContext.Delete<T>(query.GetSqlQuery(DbContext));
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entityList"></param>
        public virtual void BatchDelete(List<T> entityList)
        {
            //自动新增事务处理
            using (var tran = DbContext.DbTransaction)
            {
                DbContext.DeleteBatch<T>(entityList, tran);
                tran.Commit();
            }
        }

        #endregion

        #region  查询

        /// <summary>
        /// 获得单个实体
        /// </summary>
        /// <param name="expr">查询对象</param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual T GetEntity(DapperExQuery<T> query)
        {
            return DbContext.SingleOrDefault<T>(query.GetSqlQuery(DbContext));
        }

        /// <summary>
        ///  获取指定条件的实体集合
        /// </summary>
        /// <param name="queryList"></param>
        /// <returns></returns>
        public virtual List<T> GetAllList(string orderString = "")
        {

            if (!string.IsNullOrEmpty(orderString))
            {
                DapperExQuery<T> query = new DapperExQuery<T>();
                query.SetOrder(orderString);
                return DbContext.QueryData<T>(query.GetSqlQuery(DbContext));
            }
            else
            {
                return DbContext.QueryData<T>();
            }

        }


        /// <summary>
        ///  获取指定条件的实体集合
        /// </summary>
        /// <param name="queryList"></param>
        /// <returns></returns>
        public virtual List<T> GetList(DapperExQuery<T> query, string orderString = "")
        {
            if (!string.IsNullOrEmpty(orderString))
            {
                //设置Order条件
                query.SetOrder(orderString);
            }
            return DbContext.QueryData<T>(query.GetSqlQuery(DbContext));
        }

        /// <summary>
        /// 分页_获取指定条件的数据集合
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageIndex">开始位置（默认从1开始）</param>
        /// <param name="pageSize">当前页数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="dataCount">总查询数</param>
        /// <returns></returns>
        public virtual List<T> GetList(DapperExQuery<T> query, string orderString, int pageIndex, int pageSize, out long dataCount)
        {
            //设置Order条件
            query.SetOrder(orderString);
            return DbContext.PageData<T>(pageIndex, pageSize, out dataCount, query.GetSqlQuery(DbContext));
        }

        /// <summary>
        /// 分页_获取指定条件的数据集合
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageIndex">开始位置（默认从1开始）</param>
        /// <param name="pageSize">当前页数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="dataCount">总查询数</param>
        /// <returns></returns>
        public virtual List<T> GetListByRowNumber(DapperExQuery<T> query, string orderString, int startRowNumber, int endRowNumber, out long dataCount)
        {
            //设置Order条件
            query.SetOrder(orderString);
            return DbContext.PageDataByRowNumber<T>(startRowNumber, endRowNumber, out dataCount, query.GetSqlQuery(DbContext));
        }


        /// <summary>
        /// 统计指定条件的数据量
        /// </summary>
        /// <param name="queryList"></param>
        /// <returns></returns>
        public virtual long GetCount(DapperExQuery<T> query)
        {
            return DbContext.Count<T>(query.GetSqlQuery(DbContext));
        }

        /// <summary>
        /// 获得子查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual InWhereQuery GetInWhereSql(string selectField, DapperExQuery<T> query)
        {
            SqlQuery sql = query.GetSqlQuery(DbContext);
            return query.TranInWhereQuery(selectField, sql);
        }

        /// <summary>
        /// 通过存储过程获取数据集合
        /// </summary>
        /// <param name="storeName">存储过程名称</param>
        /// <param name="param">参数集合</param>
        /// <returns></returns>
        public virtual List<T> GetListByParams(string storeName, object param)
        {
            return DbContext.ExecuteStoredProcedureWithParms<T>(storeName, param);
        }

        /// <summary>
        /// 执行存储过程(暂时)
        /// </summary>
        /// <param name="storeName">存储过程名称</param>
        /// <param name="param">参数集合</param>
        public virtual void ExecuteStoredProcedures(string storeName, object param)
        {
            DbContext.ExecuteStoredProcedure(storeName, param);
        }

        #endregion


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
