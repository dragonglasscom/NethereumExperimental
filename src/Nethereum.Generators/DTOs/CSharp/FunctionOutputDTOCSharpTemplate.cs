using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;
using Nethereum.Generators.Model;

namespace Nethereum.Generators.DTOs
{
    public class FunctionOutputDTOCSharpTemplate: ClassTemplateBase<FunctionOutputDTOModel>
    {
        private ParameterABIFunctionDTOCSharpTemplate _parameterAbiFunctionDtocSharpTemplate;
        public FunctionOutputDTOCSharpTemplate(FunctionOutputDTOModel model):base(model)
        {
            _parameterAbiFunctionDtocSharpTemplate = new ParameterABIFunctionDTOCSharpTemplate();
            ClassFileTemplate = new CSharpClassFileTemplate(Model, this);
        }

        public override string GenerateClass()
        {
            if (Model.CanGenerateOutputDTO())
            {
                return
                    $@"{SpaceUtils.OneTab}[FunctionOutput]
{SpaceUtils.OneTab}public class {Model.GetTypeName()}
{SpaceUtils.OneTab}{{
{_parameterAbiFunctionDtocSharpTemplate.GenerateAllProperties(Model.FunctionABI.OutputParameters)}
{SpaceUtils.OneTab}}}";
            }
            return null;
        }
    }
}