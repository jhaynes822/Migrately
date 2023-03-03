import React from "react";
import { useState } from "react";
import { useDropzone } from "react-dropzone";
import PropTypes from "prop-types";
import debug from "sabio-debug";
import Toastr from "toastr";
import "../fileUpload/fileupload.css";
import * as fileUploadService from "../../services/fileUploadService";

const FileUpload = ({ onUploadSuccess }) => {
  const _logger = debug.extend("FileUpload");

  const [file, setFile] = useState([{ file: "" }]);

  const { acceptedFiles, getRootProps, getInputProps } = useDropzone({
    onDropAccepted: (acceptedFiles) => setFile(acceptedFiles),
  });

  const files = acceptedFiles.map((file) => (
    <li key={file.path}>
      {file.path} - {file.size} bytes
    </li>
  ));

  const onSubmitClicked = async (event) => {
    event.preventDefault();
    const formData = new FormData();
    const filePackage = [];

    for (let index = 0; index < file.length; index++) {
      const singleFile = file[index];
      formData.append("file", singleFile);
      formData.append("fileName", singleFile.name);
      filePackage.push(formData);
    }

    fileUploadService
      .UploadFile(formData)
      .then(onFileSubmitSuccess)
      .catch(onFileSubmitError);
  };

  const onFileSubmitSuccess = (response) => {
    Toastr.success("File uploaded successfully.", "Success");
    _logger("SUCCESS", response.data.item);
    if (onUploadSuccess) {
      onUploadSuccess(response.data.item);
    }
  };

  const onFileSubmitError = (error) => {
    Toastr.error("Error uploading file.", "Error");
    _logger("ERROR", error);
  };

  return (
    <section className="container">
      <div {...getRootProps({ className: "fileUpload" })}>
        <input {...getInputProps()} />
        <p className="file-upload-dropzone">
          Drag and drop some files here, or click to select files
        </p>
      </div>
      <aside>
        <h4>Files</h4>
        <ul>{files}</ul>
        <button
          type="submit"
          className="btn btn-outline-dark"
          onClick={onSubmitClicked}
        >
          Upload
        </button>
      </aside>
    </section>
  );
};

FileUpload.propTypes = {
  onUploadSuccess: PropTypes.func,
};

export default FileUpload;
