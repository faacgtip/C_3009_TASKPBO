using System;
using System.Collections.Generic;
using System.Globalization;

namespace SistemKaryawan
{
    // KELAS DASAR
    public class Karyawan
    {
        public string Nama { get; set; }
        public double Gaji { get; set; }

        public Karyawan(string nama, double gaji)
        {
            Nama = nama;
            Gaji = gaji;
        }

        // Format uang: 15000000 menjadi Rp15,000,000
        protected static string Rupiah(double nilai)
        {
            return "Rp" + nilai.ToString("N0", CultureInfo.InvariantCulture);
        }

        public virtual void Kerja()
        {
            Console.WriteLine($"{Nama} sedang bekerja.");
        }

        public virtual void InfoKaryawan()
        {
            Console.WriteLine($"Nama: {Nama} | Gaji: {Rupiah(Gaji)}");
        }
    }

    // KARYAWAN TETAP
    public class Tetap : Karyawan
    {
        public double Tunjangan { get; set; }

        public Tetap(string nama, double gaji, double tunjangan) : base(nama, gaji)
        {
            Tunjangan = tunjangan;
        }

        public double HitungGajiTotal()
        {
            return Gaji + Tunjangan;
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Karyawan Tetap) bekerja penuh waktu di kantor.");
        }

        public override void InfoKaryawan()
        {
            Console.WriteLine(
                $"Nama: {Nama} | Gaji: {Rupiah(Gaji)} | Tunjangan: {Rupiah(Tunjangan)} | Total: {Rupiah(HitungGajiTotal())}");
        }
    }

    public class Manager : Tetap
    {
        public Manager(string nama, double gaji, double tunjangan) : base(nama, gaji, tunjangan) { }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Manager) mengelola tim dan membuat keputusan strategis.");
        }

        public void Memimpin()
        {
            Console.WriteLine($"{Nama} memimpin rapat dan mengarahkan tim.");
        }
    }

    public class Staff : Tetap
    {
        public Staff(string nama, double gaji, double tunjangan) : base(nama, gaji, tunjangan) { }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Staff) mengerjakan tugas operasional.");
        }

        public void KerjakanTugas()
        {
            Console.WriteLine($"{Nama} menyelesaikan tugas harian yang diberikan atasan.");
        }
    }

    // KARYAWAN KONTRAK
    public class Kontrak : Karyawan
    {
        public int Durasi { get; set; }   // dalam bulan

        public Kontrak(string nama, double gaji, int durasi) : base(nama, gaji)
        {
            Durasi = durasi;
        }

        public void CekKontrak()
        {
            string status = Durasi > 6 ? "jangka panjang" : "jangka pendek";
            Console.WriteLine($"Kontrak {Nama} berdurasi {Durasi} bulan ({status}).");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Karyawan Kontrak) bekerja sesuai masa kontrak.");
        }

        public override void InfoKaryawan()
        {
            Console.WriteLine($"Nama: {Nama} | Gaji: {Rupiah(Gaji)} | Durasi Kontrak: {Durasi} bulan");
        }
    }

    public class Magang : Kontrak
    {
        public Magang(string nama, double gaji, int durasi) : base(nama, gaji, durasi) { }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Magang) membantu pekerjaan dasar tim.");
        }

        public void Belajar()
        {
            Console.WriteLine($"{Nama} sedang belajar dan dibimbing oleh mentor.");
        }
    }

    public class Freelancer : Kontrak
    {
        public Freelancer(string nama, double gaji, int durasi) : base(nama, gaji, durasi) { }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Freelancer) mengerjakan proyek secara mandiri.");
        }

        public void AmbilProyek()
        {
            Console.WriteLine($"{Nama} mengambil proyek baru dari klien.");
        }
    }

    // PERUSAHAAN (menyimpan koleksi Karyawan)
    public class Perusahaan
    {
        private List<Karyawan> daftarKaryawan = new List<Karyawan>();

        public List<Karyawan> Semua
        {
            get { return daftarKaryawan; }
        }

        public void TambahKaryawan(Karyawan karyawan)
        {
            daftarKaryawan.Add(karyawan);
            Console.WriteLine($"{karyawan.Nama} berhasil ditambahkan ke perusahaan.");
        }

        public void DaftarKaryawan()
        {
            int nomor = 1;
            foreach (Karyawan k in daftarKaryawan)
            {
                Console.Write($"{nomor}. ");
                k.InfoKaryawan();
                nomor++;
            }
        }
    }

    // PROGRAM UTAMA
    class Program
    {
        static void Judul(string teks)
        {
            Console.WriteLine();
            Console.WriteLine("[ " + teks + " ]");
        }

        static void Main(string[] args)
        {
            // a. Buat objek perusahaan
            Perusahaan perusahaan = new Perusahaan();

            // b. Buat beberapa objek karyawan
            Manager budi = new Manager("Budi", 15000000, 5000000);
            Staff andi = new Staff("Andi", 6500000, 1000000);
            Magang dina = new Magang("Dina", 1500000, 6);
            Freelancer rian = new Freelancer("Rian", 4000000, 3);

            // c. Tambahkan ke perusahaan
            Judul("TAMBAH KARYAWAN");
            perusahaan.TambahKaryawan(budi);
            perusahaan.TambahKaryawan(andi);
            perusahaan.TambahKaryawan(dina);
            perusahaan.TambahKaryawan(rian);

            // d. Tampilkan semua data
            Judul("DAFTAR KARYAWAN");
            perusahaan.DaftarKaryawan();

            // e. Demonstrasi polymorphism: satu perulangan, perilaku berbeda-beda
            Judul("POLYMORPHISM: Kerja() DI SEMUA KARYAWAN");
            foreach (Karyawan k in perusahaan.Semua)
            {
                k.Kerja();
            }

            // f. Panggil method khusus (perlu cek tipe objek asli lebih dulu)
            Judul("METHOD KHUSUS");
            foreach (Karyawan k in perusahaan.Semua)
            {
                if (k is Manager m) m.Memimpin();
                if (k is Staff s) s.KerjakanTugas();
                if (k is Magang mg) mg.Belajar();
                if (k is Freelancer f) f.AmbilProyek();
                if (k is Kontrak kt) kt.CekKontrak();
            }

            // JAWABAN SOAL

            Judul("SOAL 1: Kerja() pada Manager dan Freelancer");
            budi.Kerja();
            rian.Kerja();

            Judul("SOAL 2: Memimpin() pada Manager");
            budi.Memimpin();

            Judul("SOAL 3: Informasi lengkap Manager");
            Console.WriteLine("Cara 1 (InfoKaryawan):");
            budi.InfoKaryawan();
            Console.WriteLine("Cara 2 (akses properti satu per satu):");
            Console.WriteLine($"Nama: {budi.Nama}");
            Console.WriteLine($"Gaji: {budi.Gaji}");
            Console.WriteLine($"Tunjangan: {budi.Tunjangan}");

            Judul("SOAL 4: Belajar() pada Magang");
            dina.Belajar();

            Judul("SOAL 5: Variabel Karyawan berisi objek Staff");
            Karyawan k5 = new Staff("Andi", 6500000, 1000000);
            k5.Kerja();
            // k5.KerjakanTugas();   // ERROR CS1061: tipe variabel Karyawan tidak punya method ini
            ((Staff)k5).KerjakanTugas();   // bisa, setelah di-cast ke Staff

            Console.WriteLine();
            Console.WriteLine("Tekan tombol apa saja untuk keluar...");
            Console.ReadKey();
        }
    }
}