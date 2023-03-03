import React, { useState, useEffect } from "react";
import DatePicker from "react-flatpickr";
import { Card, Col, Image, Row } from "react-bootstrap";
import { Link } from "react-router-dom";
import { Formik, ErrorMessage, Field, Form as FormikForm } from "formik";
import { useNavigate } from "react-router-dom";
import swal from "sweetalert2";
import insuranceQuoteService from "./../../services/insuranceQuoteService";
import lookUpService from "./../../services/lookUpService";
import addInsuranceQuoteSchema from "./../../schemas/addInsuranceQuoteSchema";
import migratelyLogo from "../../assets/images/migrately/migrately-logo.png";

const InsuranceQuoteForm = () => {
  const [insuranceQuoteFormData] = useState({
    insuranceId: 0,
    coverageStartDate: new Date(),
    coverageEndDate: new Date(),
    citizenship: 0,
    age: "",
    mailingAddress: 0,
    travelDestination: 0,
    policyRangeId: 0,
    isArrivedInUSA: false,
    visaTypeId: 0,
  });

  const [insurance, setInsurance] = useState([]);
  const [lookUp, setLookUp] = useState({
    policyRange: [],
    visaTypes: [],
  });
  const [state, setState] = useState([]);
  const [country, setCountries] = useState([]);

  useEffect(() => {
    lookUpService
      .LookUp(["PolicyRange", "VisaTypes"])
      .then(lookupSuccess)
      .catch(onLookUpFail);

    lookUpService
      .LookUp3Col(["States"])
      .then(onGetStatesSuccess)
      .catch(onGetStatesFail);
    insuranceQuoteService
      .getInsurance()
      .then(onGetInsuranceSuccess)
      .catch(onGetInsuranceFail);
    insuranceQuoteService
      .getCitizenCountries()
      .then(onGetCountriesSuccess)
      .catch(onGetCountriesFailure);
  }, []);

  const lookupSuccess = (response) => {
    if (response?.item) {
      const policyRange = response.item.policyRange;
      const visaTypes = response.item.visaTypes;
      setLookUp((prevState) => {
        const newLookUp = { ...prevState };
        newLookUp.policyRange = policyRange.map(mapPolicyRange);
        newLookUp.visaTypes = visaTypes.map(mapVisaTypes);
        return newLookUp;
      });
    }
  };

  const onLookUpFail = () => {
    swal.fire("Error", "Lookup Not Found.");
  };

  const onGetStatesSuccess = (response) => {
    if (response?.items) {
      const states = response.items;
      setState(() => {
        const listOfStates = states.map(mapOfStates);
        return listOfStates;
      });
    }
  };

  const onGetStatesFail = () => {
    swal.fire("Error", "States Not Found.");
  };

  const onGetInsuranceSuccess = (response) => {
    if (response?.item) {
      const insurances = response.item;
      setInsurance(() => {
        const listOfInsurances = insurances.map(mapInsurance);
        return listOfInsurances;
      });
    }
  };

  const onGetInsuranceFail = () => {
    swal.fire("Error", "Insurance Not Found.");
  };

  const onGetCountriesSuccess = (response) => {
    if (response?.item) {
      const countries = response.item;
      setCountries(() => {
        const listOfCountries = countries.map(mapOfCountries);
        return listOfCountries;
      });
    }
  };

  const onGetCountriesFailure = () => {
    swal.fire("Error", "Country Not Found.");
  };

  const handleSubmit = (values) => {
    const insurancePayload = {
      insuranceId: parseInt(values?.insurance),
      coverageStartDate: new Date(values?.coverageStartDate),
      coverageEndDate: new Date(values?.coverageEndDate),
      citizenship: parseInt(values?.citizenship),
      age: parseInt(values.age),
      mailingAddress: parseInt(values?.mailingAddress),
      travelDestination: parseInt(values?.travelDestination),
      policyRangeId: parseInt(values?.policyRange),
      isArrivedInUSA: values?.isArrivedInUSA,
      visaTypeId: parseInt(values?.visaType),
    };
    insuranceQuoteService
      .addInsuranceQuote(insurancePayload)
      .then(onAddSuccess)
      .catch(onAddFailure);
  };

  const navigate = useNavigate();

  const onAddSuccess = () => {
    swal.fire("Success", "Insurance Quote Added.");
    navigate("/insurance/quotes");
  };

  const onAddFailure = () => {
    swal.fire("Error", "Insurance Quote Failed.", "error");
  };

  const mapInsurance = (insurance) => {
    return (
      <option value={insurance.id} key={insurance.id}>
        {insurance.name}
      </option>
    );
  };

  const mapOfCountries = (country) => {
    return (
      <option value={country.id} key={country.id}>
        {country.name}
      </option>
    );
  };

  const mapOfStates = (state) => {
    return (
      <option value={state.id} key={state.id}>
        {state.name}
      </option>
    );
  };

  const mapVisaTypes = (visaTypes) => {
    return (
      <option value={visaTypes.id} key={visaTypes.id}>
        {visaTypes.name}
      </option>
    );
  };

  const mapPolicyRange = (policyRange) => {
    return (
      <option value={policyRange.id} key={policyRange.id}>
        {policyRange.name}
      </option>
    );
  };

  const onStartDateChange = (value, setFieldValue) => {
    setFieldValue("coverageStartDate", value);
  };
  const onEndDateChange = (value, setFieldValue) => {
    setFieldValue("coverageEndDate", value);
  };

  return (
    <React.Fragment>
      <div className="user-form-background">
        <Row className="align-items-center justify-content-center g-0 min-vh-100">
          <Col lg={7} md={5} className="py-8 py-xl-0">
            <Card>
              <Card.Body className="p-6">
                <div className="mb-4">
                  <div className="user-align-center">
                    <Link to="/">
                      <Image
                        src={migratelyLogo}
                        className="mb-4"
                        alt="Migrately Logo"
                      />
                    </Link>
                  </div>
                  <h1 className="mb-1 fw-bold">Get A Quote</h1>
                  <Formik
                    enableReinitialize={true}
                    initialValues={insuranceQuoteFormData}
                    onSubmit={handleSubmit}
                    validationSchema={addInsuranceQuoteSchema}
                  >
                    {({ values, setFieldValue }) => (
                      <FormikForm>
                        <div className="container text-center">
                          <Col lg={12} md={12} className="mb-3">
                            <div className="row">
                              <div className="col">
                                <label
                                  htmlFor="insurance"
                                  className="form-label"
                                >
                                  Insurance
                                </label>
                                <Field
                                  as="select"
                                  className="form-control"
                                  id="insurance"
                                  name="insurance"
                                >
                                  <option>Choose Your Insurance</option>
                                  {insurance.length > 0 && insurance}
                                </Field>
                                <ErrorMessage
                                  name="insurance"
                                  className="has-error"
                                  component="div"
                                />
                              </div>
                            </div>
                          </Col>
                          <Col lg={12} md={12} className="mb-3">
                            <div className="row">
                              <div className="col">
                                <label
                                  htmlFor="coverageStartDate"
                                  className="form-label"
                                >
                                  Coverage Start Date
                                </label>
                                <DatePicker
                                  value={values.coverageStartDate}
                                  onChange={(date) =>
                                    onStartDateChange(date, setFieldValue)
                                  }
                                  name="coverageStartDate"
                                  className="form-control"
                                  aria-label=".form-select-lg example"
                                  id="datePicker"
                                />
                                <ErrorMessage
                                  name="coverageStartDate"
                                  className="has-error"
                                  component="div"
                                />
                              </div>
                              <div className="col">
                                <label
                                  htmlFor="coverageEndDate"
                                  className="form-label"
                                >
                                  Coverage End Date
                                </label>
                                <DatePicker
                                  value={values.coverageEndDate}
                                  name="coverageEndDate"
                                  onChange={(date) =>
                                    onEndDateChange(date, setFieldValue)
                                  }
                                  className="form-control"
                                  aria-label=".form-select-lg example"
                                />
                                <ErrorMessage
                                  name="coverageEndDate"
                                  className="has-error"
                                  component="div"
                                />
                              </div>
                            </div>
                          </Col>
                          <Col lg={12} md={12} className="mb-3">
                            <div className="row">
                              <div className="col">
                                <label
                                  htmlFor="citizenship"
                                  className="form-label"
                                >
                                  Citizenship
                                </label>
                                <Field
                                  as="select"
                                  id="citizenship"
                                  name="citizenship"
                                  className="form-control"
                                >
                                  <option>Select Country</option>
                                  {country.length > 0 && country}
                                </Field>
                                <ErrorMessage
                                  name="citizenship"
                                  className="has-error"
                                  component="div"
                                />
                              </div>

                              <div className="col">
                                <label htmlFor="age" className="form-label">
                                  Age
                                </label>
                                <Field
                                  type="text"
                                  name="age"
                                  className="form-control"
                                />
                                <ErrorMessage
                                  name="age"
                                  className="has-error"
                                  component="div"
                                />
                              </div>
                            </div>
                          </Col>
                          <Col lg={12} md={12} className="mb-3">
                            <div className="row">
                              <div className="col">
                                <label
                                  htmlFor="mailingAddress"
                                  className="form-label"
                                >
                                  Mailing Address State
                                </label>
                                <Field
                                  as="select"
                                  id="mailingAddress"
                                  name="mailingAddress"
                                  className="form-control"
                                >
                                  <option>State of Mailing Address</option>
                                  {state.length > 0 && state}
                                </Field>
                                <ErrorMessage
                                  name="mailingAddress"
                                  className="has-error"
                                  component="div"
                                />
                              </div>

                              <div className="col">
                                <label
                                  htmlFor="travelDestination"
                                  className="form-label"
                                >
                                  Travel Destination
                                </label>
                                <Field
                                  as="select"
                                  id="travelDestination"
                                  name="travelDestination"
                                  className="form-control"
                                >
                                  <option>Select Destination</option>
                                  {state.length > 0 && state}
                                </Field>
                                <ErrorMessage
                                  name="travelDestination"
                                  className="has-error"
                                  component="div"
                                />
                              </div>
                            </div>
                          </Col>
                          <Col lg={12} md={12} className="mb-3">
                            <div className="row">
                              <div className="col">
                                <label
                                  htmlFor="policyRange"
                                  className="form-label"
                                >
                                  Policy Range
                                </label>
                                <Field
                                  as="select"
                                  className="form-control"
                                  aria-label=".form-select-lg example"
                                  id="policyRange"
                                  name="policyRange"
                                >
                                  <option>Choose a Policy Range</option>
                                  {lookUp.policyRange.length > 0 &&
                                    lookUp.policyRange}
                                </Field>
                                <ErrorMessage
                                  name="policyRange"
                                  className="has-error"
                                  component="div"
                                />
                              </div>
                              <div className="col">
                                <label
                                  htmlFor="visaType"
                                  className="form-label"
                                >
                                  Visa Type
                                </label>
                                <Field
                                  as="select"
                                  className="form-control"
                                  aria-label=".form-select-lg example"
                                  id="visaType"
                                  name="visaType"
                                >
                                  <option>Choose Your Visa Type</option>
                                  {lookUp.visaTypes.length > 0 &&
                                    lookUp.visaTypes}
                                </Field>
                                <ErrorMessage
                                  name="visaType"
                                  className="has-error"
                                  component="div"
                                />
                              </div>
                            </div>
                            <div className="col">
                              <Col lg={2} md={2} className="mt-3">
                                <label className="user-form-label-text ">
                                  Currently in the USA
                                </label>
                                <div className="form-check form-switch ms-2">
                                  <Field
                                    type="checkbox"
                                    className="form-check-input"
                                    name="isArrivedInUSA"
                                  />
                                  <ErrorMessage
                                    name="isArrivedInUSA"
                                    className="has-error"
                                    component="div"
                                  />
                                </div>
                              </Col>
                            </div>
                          </Col>
                          <button className="btn btn-primary" type="submit">
                            Submit
                          </button>
                        </div>
                      </FormikForm>
                    )}
                  </Formik>
                </div>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </div>
    </React.Fragment>
  );
};

export default InsuranceQuoteForm;
