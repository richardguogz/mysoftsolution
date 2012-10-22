using System;

namespace MySoft.Data
{
    /// <summary>
    /// ��������
    /// </summary>
    [Serializable]
    public class OrderByClip
    {
        /// <summary>
        /// Ĭ�ϵ����������
        /// </summary>
        public static readonly OrderByClip None = new OrderByClip((string)null);
        private string orderBy;
        /// <summary>
        /// OrderBy��ֵ
        /// </summary>
        public string Value
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        /// <summary>
        /// �Զ���һ��OrderBy����
        /// </summary>
        public OrderByClip() { }

        /// <summary>
        /// �Զ���һ��OrderBy����
        /// </summary>
        /// <param name="orderBy"></param>
        public OrderByClip(string orderBy)
        {
            this.orderBy = orderBy;
        }

        public static bool operator true(OrderByClip right)
        {
            return false;
        }

        public static bool operator false(OrderByClip right)
        {
            return false;
        }

        public static OrderByClip operator &(OrderByClip leftOrder, OrderByClip rightOrder)
        {
            if (DataHelper.IsNullOrEmpty(leftOrder) && DataHelper.IsNullOrEmpty(rightOrder))
            {
                return OrderByClip.None;
            }
            if (DataHelper.IsNullOrEmpty(leftOrder))
            {
                return rightOrder;
            }
            if (DataHelper.IsNullOrEmpty(rightOrder))
            {
                return leftOrder;
            }
            return new OrderByClip(leftOrder.ToString() + "," + rightOrder.ToString());
        }

        public override string ToString()
        {
            return orderBy;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}