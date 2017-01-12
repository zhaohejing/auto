using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.EntityModel {
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class IcCard :BasicModel{
        [StringLength(50)]

        public string inCard { get; set; }
        [StringLength(50)]

        public string outCard { get; set; }

        public decimal Balance { get; set; }
        public int Integral { get; set; }

        public CardState State { get; set; }
    }
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class Point: BasicModel {
        public string PointName { get; set; }
        public int pointId { get; set; }
    }

    public class TempOrder {
        public int Id { get; set; }
        public int DishNumber { get; set; }
        public decimal DishCost { get; set; }
        public OrderState State { get; set; }
        public DateTime OrderTime { get; set; }
        public string Dish { get; set; }
    }
    public class CreateOrderInput {
        public int DishId { get; set; }
        public long CreateUserId { get; set; }
        public OrderState State { get; set; }
        public decimal OrderAmount{ get; set; }
        public decimal AmountPayable { get; set; }
        public int PointId { get; set; }
        public string PointName { get; set; }
        public int CustomerId { get; set; }
        public int Card { get; set; }
        public PayState PayType { get; set; }

        public DateTime OrderTime { get; set; }
        public string Dish { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
    }
  
}
