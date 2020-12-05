using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class PackageController
    {
        private IPackageService _packageService;
        public CardPackage Package { get; set; }

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public string CreatePackage(PackageDTO packageDto)
        {
            return _packageService.CreatePackage(packageDto);
        }

        public string DeletePackage(PackageDTO packageDto)
        {
            return "POST ERR - No delete";
        }
    }
}
