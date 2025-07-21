using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ClientMessages
{
    public static class ProgramMessages
    {
        public const string ProgramNotFound = "Program Bulunamadı.!";
        public const string ProgramNotCreated = "Program Oluşturulamadı.!";
        public const string ProgramCreated = "Program Oluşturuldu.!";
        public const string ProgramNotUpdated = "Program Güncellenemedi.!";
        public const string ProgramUpdated = "Program Güncellendi.!";
        public const string ProgramNotDeleted = "Program Silinemedi.!";
        public const string ProgramDeleted = "Program Silindi.!";
        public const string ProgramNotListed = "Program Listelenemedi.!";
        public const string ProgramListed = "Program Listelendi.!";
        public const string ProgramIsNotNull = "Program Boş Olamaz.!";
        public const string ProgramNameLength = "Minimum 3 Karakter Olmalı ve En Fazla 100 Karakter Olmalıdır.!";
        public const string ProgramNameExists = "Program Adı Kullanılmaktadır.!";
    }
}
