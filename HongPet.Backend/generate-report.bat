@echo off
setlocal

REM Lấy đường dẫn của thư mục chứa file .bat (của project gốc)
set BASE_DIR=%~dp0

REM Đường dẫn tương đối đến file coverage
set COVERAGE_XML=%BASE_DIR%TestResults\coverage.coveragexml

REM Đường dẫn thư mục lưu báo cáo HTML
set REPORT_DIR=%BASE_DIR%TestResults\coverage-report

REM Kiểm tra nếu file coverage.xml tồn tại
if not exist "%COVERAGE_XML%" (
    echo File coverage.coveragexml not found!
    pause
    exit /b 1
)

REM Tạo thư mục lưu báo cáo nếu chưa có
if not exist "%REPORT_DIR%" (
    mkdir "%REPORT_DIR%"
)

REM Sử dụng reportgenerator để tạo báo cáo HTML
reportgenerator -reports:"%COVERAGE_XML%" -targetdir:"%REPORT_DIR%" -reporttypes:Html

echo.
echo Generate coverage report successfully!
echo Report generated at %REPORT_DIR%\index.html
pause
