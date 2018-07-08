﻿import * as fs from 'fs';
import * as path from 'path';
import * as fsex from 'fs-extra';
import * as abiDes from "./AbiDeserialiser"

var n = require('./Nethereum.Generators.DuoCode.js');

 function generateAllClassesInternal(abi: string, byteCode: string,
    contractName: string,
    baseNamespace: string,
    serviceNamespace: string,
    cqsNamespace: string,
    dtoNamespace: string,
    basePath: string,
    pathSeparator: string,
    codeGenLang: int
) {
    var contractDes = abiDes.buildContract(abi);
    var classGenerator = new Nethereum.Generators.ContractProjectGenerator(contractDes,
        contractName,
        byteCode,
        baseNamespace,
        serviceNamespace,
        cqsNamespace,
        dtoNamespace,
        basePath,
        pathSeparator,
        codeGenLang);
    var generatedClases = classGenerator.GenerateAll();
    outputFiles(generatedClases);
}

function outputFiles(generatedFiles: Nethereum.Generators.Core.GeneratedFile[]) {
    for (var i = 0; i < generatedFiles.length; i++) {
        outputFile(generatedFiles[i]);
    }
}

function outputFile(generatedFile: Nethereum.Generators.Core.GeneratedFile) {

    fsex.ensureDirSync(generatedFile.get_OutputFolder());
    var fullPath = path.join(generatedFile.get_OutputFolder(), generatedFile.get_FileName());

    if (fs.existsSync(fullPath)) {
        fs.unlinkSync(fullPath);
    }
    fs.writeFileSync(fullPath, generatedFile.get_GeneratedCode());
}

export function  generateNetStandardClassLibrary(projectName: string, basePath: string, codeLang: int) {
    var projectGenerator = new Nethereum.Generators.NetStandardLibraryGenerator(projectName, codeLang);
    var generatedProject = projectGenerator.GenerateFileContent(basePath);
    outputFile(generatedProject);
}

export function generateAllClasses(abi: string, byteCode: string,
    contractName: string,
    baseNamespace: string,
    basePath: string,
    codeGenLang: int
) {

    var serviceNamespace = contractName + ".Service";
    var cqsNamespace = contractName + ".CQS";
    var dtoNamespace = contractName + ".DTOs";
    var pathSeparator = path.sep;
    generateAllClassesInternal(abi,
        byteCode,
        contractName,
        baseNamespace,
        serviceNamespace,
        cqsNamespace,
        dtoNamespace,
        basePath,
        pathSeparator,
        codeGenLang);
}

