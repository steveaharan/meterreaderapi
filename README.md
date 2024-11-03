# meterreaderapi

Service to load a CSV file of meter readings and store in the MeterReading table after successful validation.

Can be tested with the Meter_Reading.csv file, which has some extra rows over and above the supplied test file, which more thoroughly tests the validations.

Swagger page: http://localhost:5026/index.html

Example output:

{
"successCount": 5,
"failedCount": 33,
"failedReadings": [
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2344, MeterReadValue: 1002, ReadingDateTime: 22/04/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2233, MeterReadValue: 323, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 8766, MeterReadValue: 3440, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2344, MeterReadValue: 1002, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2346, MeterReadValue: 999999, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2347, MeterReadValue: 54, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2348, MeterReadValue: 123, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2349, MeterReadValue: VOID, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2350, MeterReadValue: 5684, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2352, MeterReadValue: 455, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2353, MeterReadValue: 1212, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2354, MeterReadValue: 889, ReadingDateTime: 22/04/2019 12:25)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2355, MeterReadValue: 1, ReadingDateTime: 06/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2356, MeterReadValue: 0, ReadingDateTime: 07/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 2344, MeterReadValue: 0X765, ReadingDateTime: 08/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 6776, MeterReadValue: -6575, ReadingDateTime: 09/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 4534, MeterReadValue: , ReadingDateTime: 11/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1234, MeterReadValue: 9787, ReadingDateTime: 12/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1235, MeterReadValue: , ReadingDateTime: 13/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1236, MeterReadValue: 8898, ReadingDateTime: 10/04/2019 19:34)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1237, MeterReadValue: 3455, ReadingDateTime: 15/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1238, MeterReadValue: 0, ReadingDateTime: 16/05/2019 09:24)",
"Reading date and time cannot be in the future. (AccountId: 1239, MeterReadValue: 45345, ReadingDateTime: 17/05/2030 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1240, MeterReadValue: 978, ReadingDateTime: 18/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1241, MeterReadValue: 436, ReadingDateTime: 11/04/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1242, MeterReadValue: 124, ReadingDateTime: 20/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1243, MeterReadValue: 77, ReadingDateTime: 21/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1244, MeterReadValue: 3478, ReadingDateTime: 25/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1245, MeterReadValue: 676, ReadingDateTime: 25/05/2019 14:26)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1246, MeterReadValue: 3455, ReadingDateTime: 25/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1247, MeterReadValue: 3, ReadingDateTime: 25/05/2019 09:24)",
"Invalid meter reading value. It must be exactly 5 numeric characters. (AccountId: 1248, MeterReadValue: 3467, ReadingDateTime: 26/05/2019 09:24)",
"Account does not exist. (AccountId: 11248, MeterReadValue: 12345, ReadingDateTime: 26/05/2019 09:24)"
]
}

# ClientApp

Built in Angular, VERY basic MVP UI to allow a user to select a file and post it to the api.

To run:

npm install
npm run start

Open http://localhost:4200/ in your browser
