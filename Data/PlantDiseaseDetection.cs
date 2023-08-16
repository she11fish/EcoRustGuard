namespace Plant_Disease_Classification.Data;
using Microsoft.ML;

public class PlantDiseaseDetection : FileSystem
{
    public ITransformer LoadModel(MLContext mlContext) 
    {
        return mlContext.Model.Load("./Data/model.zip", out _);
    }

    public IDataView PreprocessData(MLContext mlContext, IDataView imageData, string path)
    {
        // Sets the preprocesing pipline to preprocess data
        var preprocessingPipeline = mlContext.Transforms.Conversion.MapValueToKey(
                    inputColumnName: "Label",
                    outputColumnName: "LabelAsKey")
                .Append(mlContext.Transforms.LoadRawImageBytes(
                    outputColumnName: "Image",
                    imageFolder: path,
                    inputColumnName: "ImagePath"));

        // Preprocesses data to make it suitable for creating the model input
        return preprocessingPipeline.Fit(imageData).Transform(imageData);
    }

    public string ClassifySingleImage(MLContext mlContext, IDataView data, ITransformer trainedModel)
    {
        // Set up the prediction engine
        PredictionEngine<ModelInput, ModelOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);
        
        // Sets up an image as the model input
        ModelInput image = mlContext.Data.CreateEnumerable<ModelInput>(data,reuseRowObject:true).First();
        
        // Predicts if the plant is healthy or powdery (ill) from the trained model
        ModelOutput prediction = predictionEngine.Predict(image);

        return prediction.PredictedLabel;
    }
}
