# ⚓ Mini Battleship (battleship)

Một bản demo của trò chơi "Battleship" (Bắn tàu) cổ điển được xây dựng bằng C# và Windows Forms trên nền tảng .NET Framework.

## 📝 Mô tả

Dự án này là một phiên bản "mini" của trò chơi Battleship, chơi trên lưới 7x7. Người chơi có thể chọn giữa hai chế độ: chiến đấu với máy (BOT) hoặc chơi cùng một người khác trên cùng một máy (chế độ "hot-seat").

Mục tiêu là đoán vị trí tàu của đối thủ và bắn hạ toàn bộ hạm đội của họ trước khi họ làm điều tương tự với bạn.

## ✨ Tính năng

* **Lưới chơi 7x7**: Một phiên bản nhỏ gọn, nhanh chóng của trò chơi cổ điển.
* **Hạm đội ngẫu nhiên**: Mỗi trận đấu, một hạm đội gồm 3 tàu (kích thước 3, 2, và 2) sẽ được tự động đặt ngẫu nhiên trên bàn cờ.
* **Hai chế độ chơi**:
    * **🤖 Chơi với BOT**: Thử thách kỹ năng của bạn với một AI đơn giản.
    * **👥 Chơi 2 Người**: Chế độ "hot-seat" cho hai người chơi.
* **Logic PvP "Mù"**: Ở chế độ 2 người chơi, bạn không thể nhìn thấy vị trí tàu của chính mình. Bạn phải tự nhớ vị trí hoặc suy luận dựa trên các phát bắn của đối thủ, giống như khi chơi trên giấy.
* **Giao diện đồ họa**: Sử dụng Windows Forms để tạo trải nghiệm trực quan với các nút bấm và thông báo trạng thái.

## 🛠️ Công nghệ sử dụng

* **Ngôn ngữ**: C#
* **Nền tảng**: .NET Framework 4.7.2
* **Giao diện**: Windows Forms (WinExe)

## 🚀 Cách chạy dự án

Bạn sẽ cần Visual Studio (ví dụ: Visual Studio 2019 hoặc mới hơn) và .NET Framework 4.7.2 Developer Pack.

1.  **Clone repository:**
    ```bash
    git clone [(https://github.com/hoangsphan/batteshipdemo/)]
    ```
2.  **Mở Solution:**
    * Mở file `batteshipdemo.sln` bằng Visual Studio.
3.  **Build dự án:**
    * Nhấn `Build > Build Solution` (hoặc `Ctrl+Shift+B`) để Visual Studio khôi phục các gói cần thiết.
4.  **Chạy dự án:**
    * Đặt project `battleship` làm project khởi động (Startup Project).
    * Nhấn `F5` hoặc nút "Start" để chạy game. Menu chính sẽ xuất hiện.


