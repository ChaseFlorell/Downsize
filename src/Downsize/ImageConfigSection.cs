using System.Collections.Generic;
using System.Configuration;

namespace Downsize
{
    public class ImageConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("ImageCollection")]
        public ImageConfigElementCollection ImageCollection
        {
            get { return this["ImageCollection"] as ImageConfigElementCollection; }
        }

        public static ImageConfigSection GetConfigSection()
        {
            return ConfigurationManager.GetSection("ImageConfigSection") as ImageConfigSection;
        }

        public static IEnumerable<ImageConfigElement> DefaultCollection()
        {
            return new List<ImageConfigElement>
            {
                new ImageConfigElement("ldpi",@"{0}\Android\drawable-ldpi\{1}",4),
                new ImageConfigElement("mdpi",@"{0}\Android\drawable-mdpi\{1}",3),
                new ImageConfigElement("hdpi",@"{0}\Android\drawable-hdpi\{1}",2),
                new ImageConfigElement("xhdpi",@"{0}\Android\drawable-xhdpi\{1}",1.5),
                new ImageConfigElement("xxhdpi",@"{0}\Android\drawable-xxhdpi\{1}",1),
                new ImageConfigElement("iOS@1x",@"{0}\iOS\{1}",3),
                new ImageConfigElement("iOS@2x",@"{0}\iOS\{1}@2x",1.5),
                new ImageConfigElement("iOS@3x",@"{0}\iOS\{1}@3x",1)
            };
        }
    }

    public class ImageConfigElementCollection : ConfigurationElementCollection
    {
        public ImageConfigElement this[int index]
        {
            get { return BaseGet(index) as ImageConfigElement; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ImageConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ImageConfigElement) element).Key;
        }
    }

    public class ImageConfigElement : ConfigurationElement
    {
        private readonly string _key;
        private readonly string _pathFormat;
        private readonly double? _scale;

        public ImageConfigElement(string key, string pathFormat, double scale)
        {
            _key = key;
            _pathFormat = pathFormat;
            _scale = scale;
        }

        public ImageConfigElement() { }

        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_key))
                {
                    return this["key"] as string;
                }
                return _key;
            }
        }

        [ConfigurationProperty("pathFormat", IsRequired = true)]
        public string PathFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_pathFormat))
                {
                    return this["pathFormat"] as string;
                }
                return _pathFormat;
            }
        }

        [ConfigurationProperty("scale", IsRequired = true)]
        public double Scale
        {
            get
            {
                if (_scale == null)
                {
                    double scale;
                    double.TryParse(this["scale"].ToString(), out scale);
                    return scale;
                }
                return (double) _scale;
            }
        }
    }
}