//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: Reports web vitals to help make react better, I think.
//
//################################################################

// this function reports web vitals
const reportWebVitals = (onPerfEntry) => 
{
  if (onPerfEntry && onPerfEntry instanceof Function) 
  {
    import('web-vitals').then(({ getCLS, getFID, getFCP, getLCP, getTTFB }) => 
    {
      getCLS(onPerfEntry);
      getFID(onPerfEntry);
      getFCP(onPerfEntry);
      getLCP(onPerfEntry);
      getTTFB(onPerfEntry);
    });
  }
};

export default reportWebVitals;
