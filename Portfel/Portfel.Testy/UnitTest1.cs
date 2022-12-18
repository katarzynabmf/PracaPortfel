using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Portfel.Data;
using Portfel.Data.Data;
using Portfel.Data.Serwisy;

namespace Portfel.Testy
{
    public class UnitTest
    {
        [Fact]
        public void TestWplacSrodkiNaKonto()
        {
            //test wp³aty

            // given
            var portfele = new List<Data.Data.Portfel>().AsQueryable();
            //mockowanie encji Portfel
            var mockSet = new Mock<DbSet<Data.Data.Portfel>>();
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.Provider).Returns(portfele.Provider);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.Expression).Returns(portfele.Expression);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.ElementType).Returns(portfele.ElementType);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.GetEnumerator()).Returns(() => portfele.GetEnumerator());
           
            //mockowanie po³¹czenia z baz¹ danych
            var mockContext = new Mock<PortfelContext>();
            mockContext.Setup(c => c.Portfele).Returns(mockSet.Object);

            // -----------------------------------------
            // when
            var portfelSerwis = new PortfelSerwis(mockContext.Object);
            var portfel = new Data.Data.Portfel(){KontoGotowkowe = new KontoGotowkowe(){StanKonta = 0, OperacjeGotowkowe = new List<OperacjaGotowkowa>()}, Pozycje = new List<Pozycja>(), Transakcje = new List<TransakcjaNew>()};
            portfelSerwis.WplacSrodkiNaKonto(100, portfel);

            // then
            mockContext.Verify(context =>
                context.Portfele.Update(It.Is<Data.Data.Portfel>(portfel => portfel.KontoGotowkowe.StanKonta == 100)), Times.Once);
        }

        [Fact]
        public void TestWyplacSrodkiZKonta()
        {
            //test wyp³aty
            // given

            var portfele = new List<Data.Data.Portfel>().AsQueryable();
            //mockowanie encji Portfel
            var PortfeleMockSet = new Mock<DbSet<Data.Data.Portfel>>();
            PortfeleMockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.Provider).Returns(portfele.Provider);
            PortfeleMockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.Expression).Returns(portfele.Expression);
            PortfeleMockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.ElementType).Returns(portfele.ElementType);
            PortfeleMockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.GetEnumerator()).Returns(() => portfele.GetEnumerator());

            //mockowanie po³¹czenia z baz¹ danych
            var mockContext = new Mock<PortfelContext>();
            mockContext.Setup(c => c.Portfele).Returns(PortfeleMockSet.Object);

            // -----------------------------------------
            // when
            var portfelSerwis = new PortfelSerwis(mockContext.Object);
            var portfel = new Data.Data.Portfel() { KontoGotowkowe = new KontoGotowkowe() { StanKonta = 200,
                OperacjeGotowkowe = new List<OperacjaGotowkowa>() },
                Pozycje = new List<Pozycja>(),
                Transakcje = new List<TransakcjaNew>() };
            portfelSerwis.WyplacSrodkiZKonta(100, portfel);

            // then
            mockContext.Verify(context =>
                context.Portfele.Update(It.Is<Data.Data.Portfel>(portfel => portfel.KontoGotowkowe.StanKonta == 100)), Times.Once);
        }

        [Fact]
        public void TestKupAktywo()
        {
            // given
            var portfele = new List<Data.Data.Portfel>().AsQueryable();
            //mockowanie encji Portfel
            var mockSet = new Mock<DbSet<Data.Data.Portfel>>();
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.Provider).Returns(portfele.Provider);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.Expression).Returns(portfele.Expression);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.ElementType).Returns(portfele.ElementType);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.GetEnumerator()).Returns(() => portfele.GetEnumerator());

            var aktywa = new List<Aktywo>
            {
                new Aktywo{Symbol = "PKO"},
            }.AsQueryable();
            //mockowanie encji Aktywo
            var AktywaMockSet = new Mock<DbSet<Aktywo>>();
            AktywaMockSet.As<IQueryable<Aktywo>>().Setup(m => m.Provider).Returns(aktywa.Provider);
            AktywaMockSet.As<IQueryable<Aktywo>>().Setup(m => m.Expression).Returns(aktywa.Expression);
            AktywaMockSet.As<IQueryable<Aktywo>>().Setup(m => m.ElementType).Returns(aktywa.ElementType);
            AktywaMockSet.As<IQueryable<Aktywo>>().Setup(m => m.GetEnumerator()).Returns(() => aktywa.GetEnumerator());

            //mockowanie encji 
            var transakcje = new List<TransakcjaNew>().AsQueryable();
            var TransakcjeNewMockSet = new Mock<DbSet<TransakcjaNew>>();
            TransakcjeNewMockSet.As<IQueryable<TransakcjaNew>>().Setup(m => m.Provider).Returns(transakcje.Provider);
            TransakcjeNewMockSet.As<IQueryable<TransakcjaNew>>().Setup(m => m.Expression).Returns(transakcje.Expression);
            TransakcjeNewMockSet.As<IQueryable<TransakcjaNew>>().Setup(m => m.ElementType).Returns(transakcje.ElementType);
            TransakcjeNewMockSet.As<IQueryable<TransakcjaNew>>().Setup(m => m.GetEnumerator()).Returns(() => transakcje.GetEnumerator());
            
            TransakcjeNewMockSet.Setup(t => t.Add(It.IsAny<TransakcjaNew>())).Returns((TransakcjaNew transakcja) =>
            {
                var internalEntityEntry = new InternalEntityEntry(
                    new Mock<IStateManager>().Object,
                    new RuntimeEntityType("T", typeof(TransakcjaNew), false, null, null, null, ChangeTrackingStrategy.Snapshot, null, false),
                    transakcja);

                var entityEntryMock = new Mock<EntityEntry<TransakcjaNew>>(internalEntityEntry);
                entityEntryMock.Setup(entry => entry.Entity).Returns(transakcja);
                return entityEntryMock.Object;
            });

            //mockowanie po³¹czenia z baz¹ danych
            var mockContext = new Mock<PortfelContext>();
            
            mockContext.Setup(c => c.Portfele).Returns(mockSet.Object);
            mockContext.Setup(c => c.Aktywa).Returns(AktywaMockSet.Object);
            mockContext.Setup(c => c.TransakcjeNew).Returns(TransakcjeNewMockSet.Object);

            // -----------------------------------------
            // when
            var portfelSerwis = new PortfelSerwis(mockContext.Object);
            var portfel = new Data.Data.Portfel()
                {KontoGotowkowe = new KontoGotowkowe() { StanKonta = 2000, OperacjeGotowkowe = new List<OperacjaGotowkowa>() }, 
                    Pozycje = new List<Pozycja>(), 
                    Transakcje = new List<TransakcjaNew>()

                };
            portfelSerwis.KupAktywo("PKO",10,10, "", portfel);

            // then
            mockContext.Verify(context =>
                context.Portfele.Update(It.Is<Data.Data.Portfel>(portfel => portfel.KontoGotowkowe.StanKonta == 1900 )), Times.Once);
            
            mockContext.Verify(context =>
                context.Portfele.Update(It.Is<Data.Data.Portfel>(portfel => portfel.Pozycje.Count==1)), Times.Once);
        }
        [Fact]
        public void TestSprzedajAktywo()
        {
            // given
            var portfele = new List<Data.Data.Portfel>().AsQueryable();
            //mockowanie encji Portfel
            var mockSet = new Mock<DbSet<Data.Data.Portfel>>();
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.Provider).Returns(portfele.Provider);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.Expression).Returns(portfele.Expression);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.ElementType).Returns(portfele.ElementType);
            mockSet.As<IQueryable<Data.Data.Portfel>>().Setup(m => m.GetEnumerator()).Returns(() => portfele.GetEnumerator());

            var aktywa = new List<Aktywo>
            {
                new Aktywo{Symbol = "PKO"},
            }.AsQueryable();
            //mockowanie encji Aktywo
            var AktywaMockSet = new Mock<DbSet<Aktywo>>();
            AktywaMockSet.As<IQueryable<Aktywo>>().Setup(m => m.Provider).Returns(aktywa.Provider);
            AktywaMockSet.As<IQueryable<Aktywo>>().Setup(m => m.Expression).Returns(aktywa.Expression);
            AktywaMockSet.As<IQueryable<Aktywo>>().Setup(m => m.ElementType).Returns(aktywa.ElementType);
            AktywaMockSet.As<IQueryable<Aktywo>>().Setup(m => m.GetEnumerator()).Returns(() => aktywa.GetEnumerator());

            //mockowanie encji 
            var transakcje = new List<TransakcjaNew>().AsQueryable();
            var TransakcjeNewMockSet = new Mock<DbSet<TransakcjaNew>>();
            TransakcjeNewMockSet.As<IQueryable<TransakcjaNew>>().Setup(m => m.Provider).Returns(transakcje.Provider);
            TransakcjeNewMockSet.As<IQueryable<TransakcjaNew>>().Setup(m => m.Expression).Returns(transakcje.Expression);
            TransakcjeNewMockSet.As<IQueryable<TransakcjaNew>>().Setup(m => m.ElementType).Returns(transakcje.ElementType);
            TransakcjeNewMockSet.As<IQueryable<TransakcjaNew>>().Setup(m => m.GetEnumerator()).Returns(() => transakcje.GetEnumerator());

            TransakcjeNewMockSet.Setup(t => t.Add(It.IsAny<TransakcjaNew>())).Returns((TransakcjaNew transakcja) =>
            {
                var internalEntityEntry = new InternalEntityEntry(
                    new Mock<IStateManager>().Object,
                    new RuntimeEntityType("T", typeof(TransakcjaNew), false, null, null, null, ChangeTrackingStrategy.Snapshot, null, false),
                    transakcja);

                var entityEntryMock = new Mock<EntityEntry<TransakcjaNew>>(internalEntityEntry);
                entityEntryMock.Setup(entry => entry.Entity).Returns(transakcja);
                return entityEntryMock.Object;
            });

            //mockowanie po³¹czenia z baz¹ danych
            var mockContext = new Mock<PortfelContext>();

            mockContext.Setup(c => c.Portfele).Returns(mockSet.Object);
            mockContext.Setup(c => c.Aktywa).Returns(AktywaMockSet.Object);
            mockContext.Setup(c => c.TransakcjeNew).Returns(TransakcjeNewMockSet.Object);

            // -----------------------------------------
            // when
            var portfelSerwis = new PortfelSerwis(mockContext.Object);
            var portfel = new Data.Data.Portfel()
            {
                KontoGotowkowe = new KontoGotowkowe() { StanKonta = 2000, OperacjeGotowkowe = new List<OperacjaGotowkowa>() },
                Pozycje = new List<Pozycja>()
                {
                    new Pozycja()
                    {
                        Id = 0,
                        Aktywo= new Aktywo()
                        {
                            Aktywna = true,
                            CenaAktualna = 10,
                            Id = 1,
                            Nazwa = "PKO",
                            Symbol = "PKO"
                        }, 
                        Ilosc = 10, 
                        SredniaCenaZakupu = 25
                    }
                },
                Transakcje = new List<TransakcjaNew>()
            };
            portfelSerwis.SprzedajAktywo("PKO", 9, 10,  "", portfel);

            // then
            mockContext.Verify(context =>
                context.Portfele.Update(It.Is<Data.Data.Portfel>(portfel => portfel.KontoGotowkowe.StanKonta == 2090)), Times.Once);

            mockContext.Verify(context =>
                context.Portfele.Update(It.Is<Data.Data.Portfel>(portfel => portfel.Pozycje.Count == 1)), Times.Once);
        }
    }
}