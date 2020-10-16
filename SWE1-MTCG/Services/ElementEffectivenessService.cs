using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Services
{
    public class ElementEffectivenessService
    {
        public double CompareElements(ElementType element1, ElementType element2)
        {
            switch (element1)
            {
                case ElementType.Normal:
                    if (element2 is ElementType.Fire)
                    {
                        return 0.5;
                    }
                    else if (element2 is ElementType.Water)
                    {
                        return 2.0;
                    }
                    return 1.0;
                case ElementType.Fire:
                    if (element2 is ElementType.Water)
                    {
                        return 0.5;
                    }
                    else if (element2 is ElementType.Normal)
                    {
                        return 2.0;
                    }
                    return 1.0;
                case ElementType.Water:
                    if (element2 is ElementType.Normal)
                    {
                        return 0.5;
                    }
                    else if(element2 is ElementType.Fire)
                    {
                        return 2.0;
                    }

                    return 1.0;
                default:
                    return 1.0;
            }
        }
    }
}
