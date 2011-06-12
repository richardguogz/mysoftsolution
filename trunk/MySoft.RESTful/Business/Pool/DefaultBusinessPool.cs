using System;
using System.Collections.Generic;
using System.Linq;

namespace MySoft.RESTful.Business.Pool
{
    /// <summary>
    /// 默认业务池
    /// </summary>
    public class DefaultBusinessPool : IBusinessPool
    {
        private IDictionary<string, BusinessKindModel> businessPool;

        /// <summary>
        /// 获取业务池对象
        /// </summary>
        public IDictionary<string, BusinessKindModel> BusinessPool
        {
            get { return businessPool; }
        }

        /// <summary>
        /// 实例化DefaultBusinessPool
        /// </summary>
        public DefaultBusinessPool()
        {
            businessPool = new Dictionary<string, BusinessKindModel>();
        }

        /// <summary>
        /// 获取业务模型
        /// </summary>
        /// <param name="businessKindName"></param>
        /// <returns></returns>
        public BusinessKindModel GetKindModel(string businessKindName)
        {
            BusinessKindModel model = businessPool.Where(e => e.Key.Equals(businessKindName, StringComparison.OrdinalIgnoreCase)).Select(v => v.Value).SingleOrDefault();
            return model;
        }

        /// <summary>
        /// 添加业务模型
        /// </summary>
        /// <param name="businessKindName"></param>
        /// <param name="businessKindModel"></param>
        /// <returns></returns>
        public IDictionary<string, BusinessModel> AddKindModel(string businessKindName, BusinessKindModel businessKindModel)
        {
            BusinessKindModel model = businessPool.Where(e => e.Key.Equals(businessKindName, StringComparison.OrdinalIgnoreCase)).Select(v => v.Value).SingleOrDefault();
            if (model == null)
            {
                businessPool.Add(businessKindName, businessKindModel);
                return businessKindModel.Models;
            }
            return model.Models;
        }

        /// <summary>
        /// 移除业务模型
        /// </summary>
        /// <param name="businessKindName"></param>
        /// <returns></returns>
        public BusinessKindModel RemoveKindModel(string businessKindName)
        {
            BusinessKindModel model = businessPool.Where(e => e.Key.Equals(businessKindName, StringComparison.OrdinalIgnoreCase)).Select(v => v.Value).SingleOrDefault();
            if (model != null)
            {
                businessPool.Remove(businessKindName);
            }
            return model;
        }

        /// <summary>
        /// 移除业务模型
        /// </summary>
        /// <param name="businessKindName"></param>
        /// <param name="businessMethodName"></param>
        /// <returns></returns>
        public IDictionary<string, BusinessModel> RemoveBusinessModel(string businessKindName, string businessMethodName)
        {
            BusinessKindModel model = businessPool.Where(e => e.Key.Equals(businessKindName, StringComparison.OrdinalIgnoreCase)).Select(v => v.Value).SingleOrDefault();
            if (model != null)
            {
                model.Models.Remove(businessMethodName);
                return model.Models;
            }
            return null;
        }

        /// <summary>
        /// 查找方法
        /// </summary>
        /// <param name="businessKindName"></param>
        /// <param name="businessMethodName"></param>
        /// <returns></returns>
        public IList<BusinessMetadata> Find(string businessKindName, string businessMethodName)
        {
            bool hasException = false;
            string msg = string.Empty;
            RESTfulCode code = RESTfulCode.OK;
            IList<BusinessMetadata> metadatas = null;
            BusinessKindModel model = businessPool.Where(e => e.Key.Equals(businessKindName, StringComparison.OrdinalIgnoreCase)).Select(v => v.Value).SingleOrDefault();
            if (model == null)
            {
                hasException = true;
                msg = businessKindName + ", did not found!";
                code = RESTfulCode.BUSINESS_KIND_NOT_FOUND;
            }
            else
            {
                if (model.State == BusinessState.ACTIVATED)
                {
                    BusinessModel bm = model.Models.Where(e => e.Key.Equals(businessMethodName, StringComparison.OrdinalIgnoreCase)).Select(v => v.Value).SingleOrDefault();
                    if (bm == null)
                    {
                        hasException = true;
                        msg = businessMethodName + ", did not found!";
                        code = RESTfulCode.BUSINESS_METHOD_NOT_FOUND;
                    }
                    else
                    {
                        if (bm.State != BusinessState.ACTIVATED)
                        {
                            hasException = true;
                            msg = businessMethodName + ", did not Activeted!";
                            code = RESTfulCode.BUSINESS_KIND_NO_ACTIVATED;
                        }
                        else
                        {
                            metadatas = bm.Metadatas;
                        }
                    }
                }
                else
                {
                    hasException = true;
                    msg = businessKindName + ", did not Activeted!";
                    code = RESTfulCode.BUSINESS_KIND_NO_ACTIVATED;
                }
            }
            if (hasException)
            {
                throw new RESTfulException(msg) { Code = code };
            }
            else
            {
                return metadatas;
            }
        }

        /// <summary>
        /// 查找业务元素
        /// </summary>
        /// <param name="businessKindName"></param>
        /// <param name="businessMethodName"></param>
        /// <param name="paramsCount"></param>
        /// <returns></returns>
        public BusinessMetadata Find(string businessKindName, string businessMethodName, int paramsCount)
        {
            bool hasException = false;
            string msg = string.Empty;
            RESTfulCode code = RESTfulCode.OK;
            IList<BusinessMetadata> metadatas = Find(businessKindName, businessMethodName);
            BusinessMetadata metadata = metadatas.Where(e => e.ParametersCount == paramsCount).SingleOrDefault();
            if (metadata == null)
            {
                hasException = true;
                msg = businessMethodName + " and parameter count is " + paramsCount + ", did not match!";
                code = RESTfulCode.BUSINESS_METHOD_PARAMS_COUNT_NOT_MATCH;
            }
            else if (metadata.State != BusinessState.ACTIVATED)
            {
                hasException = true;
                msg = businessMethodName + " and parameter count is " + paramsCount + ", did not Activeted!";
                code = RESTfulCode.BUSINESS_METHOD_NO_ACTIVATED;
            }

            if (hasException)
            {
                throw new RESTfulException(msg) { Code = code };
            }
            else
            {
                return metadata;
            }
        }
    }
}
