﻿using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class UserRegisterView
    {
        [Required(ErrorMessage = "Vui lòng không để trống họ tên"), RegularExpression(@"(^([a-zỳọáầảấờễàạằệếýộậốũứĩõúữịỗìềểẩớặòùồợãụủíỹắẫựỉỏừỷởóéửỵẳẹèẽổẵẻỡơôưăêâđ'A-ZỲỌÁẦẢẤỜỄÀẠẰỆẾÝỘẬỐŨỨĨÕÚỮỊỖÌỀỂẨỚẶÒÙỒỢÃỤỦÍỸẮẪỰỈỎỪỶỞÓÉỬỴẲẸÈẼỔẴẺỠƠÔƯĂÊÂĐ]{1,})((\s)([a-zỳọáầảấờễàạằệếýộậốũứĩõúữịỗìềểẩớặòùồợãụủíỹắẫựỉỏừỷởóéửỵẳẹèẽổẵẻỡơôưăêâđ'A-ZỲỌÁẦẢẤỜỄÀẠẰỆẾÝỘẬỐŨỨĨÕÚỮỊỖÌỀỂẨỚẶÒÙỒỢÃỤỦÍỸẮẪỰỈỎỪỶỞÓÉỬỴẲẸÈẼỔẴẺỠƠÔƯĂÊÂĐ]{1,}))*)$", ErrorMessage = "Tên chưa đúng định dạng")]
        public string FullName { get; set; }
        public bool Gender { get; set; }    
        public string? Password { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống email")]
        [RegularExpression(@"([\w]{5,20})+@[a-z]+\.([a-z]{2,3}|[a-z]{2,3}\.[a-z]{2,3})$", ErrorMessage = "Email chưa đúng định dạng")]
        public string Email { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng không bỏ trống số điện thoại"), RegularExpression(@"(^(086|096|097|098|039|038|037|036|035|034|033|032|091|094|088|083|084|085|081|082|070|079|077|076|078|089|090|093|092|052|056|058|099|059)([0-9]{7,7}))$", ErrorMessage = "Chưa đúng định dạng số điện thoại Việt Nam")]
        public string PhoneNumber { get; set; }
    }
}
