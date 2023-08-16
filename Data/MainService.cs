namespace Plant_Disease_Classification.Data;
using Microsoft.ML;
public class MainService : PlantDiseaseDetection
{
    public string MakePrediction(IEnumerable<ImageData> images, string path)
    {
        // Set up the model
        MLContext mlContext = new MLContext();
        ITransformer mlModel = LoadModel(mlContext); 

        // Load and preprocess data
        IDataView imageData = mlContext.Data.LoadFromEnumerable(images);
        IDataView preProcessedData = PreprocessData(mlContext, imageData, path);

        return ClassifySingleImage(mlContext, preProcessedData, mlModel);;
    }
}