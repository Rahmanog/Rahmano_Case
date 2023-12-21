## Aplikasi dibuat menggunakan:
- Visual Studi 2019
  
## DataBase menggunakan :
- SqLite 3.12.2 (File di folder 'Rahmano_Case\Rahmano_Case\Data')

## Chat Feature:
- SignalR 2.4.3

//======= NOTE =======
#### Idealnya Aplikasi menggunakan DBContext. Namun mengingat keterbatasan waktu, maka untuk akses data sepenuhnya menggunakan ADO.NET 
//

## Menu:
(Home)
Memberikan penjelasan singkat mengenai Aplikasi

## A. User Akses
Bagian untuk membuat User baru (Sign in) atau Login bagi user yang telah terdata di DataBase.
Fiture:
- Sign In
- Log In
- Update Profile
- Change Password
- LogOff

## B. View Article
Bagian dimana pengguna dapat melihat artikel-artikel yang telah di publikasikan (publish). Pada bagian ini User dapat melihat Chat dari seluruh pengguna yang masuk pada artikel terkait. Dimana Chat ini bersifat real Time dan semua Chat tersimpan di dalam Database.
Sementara untuk dapat mengirimkan Chat User haris LogIn terlebih dahulu, guna pendataan Chat. Idealnya ada fitur Admin guna menghapus Chat-chat yang dianggak tidak layak.
Fiture:
- View Article
- Filter Article by Category
- Chat

## C. Create Article
Pada bagian ini User (yang sucah login) dapat membuat artikel dan menyimpannya, dan suatu saat dianggap sudah layak dapat di Publish untuk dapat dilihat semua user. List artikel yang muncul adalah Artikel yang dibuat oleh user yang bersangkutan. Artikel yang dibuat dilengkapi dengan Head Image yang berfungsi sebagai Icon berite dan isi berita dalam format HTM.
Sementara User yang belum Login tidak dapat menambah atau membuat artikel.
Fiture:
- View List Artikel
- Create New Article
- Update Article
- Publish Article
- Delete Article
  

## ******* TERIMA KASIH *******
