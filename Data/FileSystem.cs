namespace Plant_Disease_Classification.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
public class FileSystem
{
    public static IEnumerable<ImageData> LoadImagesFromDirectory(string folder, bool useFolderNameAsLabel = true)
    {
        var files = Directory.GetFiles(folder, "*");

        foreach (var file in files)
        {
            if ((Path.GetExtension(file) != ".jpg") && (Path.GetExtension(file) != ".png"))
                continue;

            var label = Path.GetFileName(file);

            if (useFolderNameAsLabel)
                label = Directory.GetParent(file).Name;
            else
            {
                for (int index = 0; index < label.Length; index++)
                {
                    if (!char.IsLetter(label[index]))
                    {
                        label = label.Substring(0, index);
                        break;
                    }
                }
            }

            yield return new ImageData()
            {
                ImagePath = file,
                Label = label
            };
        }
    }

    public static void ResizeImage(string imagePath, int width, int height)
    {
        using (var image = Image.Load(imagePath))
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Stretch
            }));

            image.Save(imagePath); // Overwrite the original image file
        }
    }
}